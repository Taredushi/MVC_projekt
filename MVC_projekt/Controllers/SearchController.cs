using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using MVC_projekt.Models;
using MVC_projekt.Models.Helpers;
using Newtonsoft.Json;
using PagedList;

namespace MVC_projekt.Controllers
{

    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ViewTools vt = new ViewTools();

        public ActionResult Author(int? page, string searchString, string submit, bool? save)
        {
            int currentPage = page ?? 1;
            int onPage = 5;

            if (!string.IsNullOrEmpty(submit) && !string.IsNullOrEmpty(searchString))
            {
                var author = db.Authors.Where(x => (x.Name.ToLower().Contains(searchString.ToLower()) || x.Surname.ToLower().Contains(searchString.ToLower())) && x.AuthorGroups.Any()).ToList();
                var authorSearchView = author.Select(a => vt.GetAuthorViewModel(a, db)).OrderBy(x => x.FullName).ToList();
                return View(authorSearchView.ToPagedList<AuthorViewModel>(currentPage, onPage));
            }
            if (save != null)
            {
                string url = Request.UrlReferrer.ToString();
                string userId = User.Identity.GetUserId();
                vt.SaveSearch(userId, url, db);
                return Redirect(url);
            }

            var authors = db.Authors.Where(x => x.AuthorGroups.Any()).ToList();
            var authorView = authors.Select(a => vt.GetAuthorViewModel(a, db)).OrderBy(x => x.FullName).ToList();

            return View(authorView.ToPagedList<AuthorViewModel>(currentPage, onPage));
        }

        //Get: Title
        public ActionResult Title(int? page, string searchString, string submit, bool? save)
        {
            int currentPage = page ?? 1;
            int onPage = 5;

            if (!string.IsNullOrEmpty(submit) && !string.IsNullOrEmpty(searchString))
            {
                var book =
                        db.BookItems.Where(x => x.Title.ToUpper().Contains(searchString.ToUpper()))
                            .GroupBy(x => x.Title)
                            .ToList();
                var bookView = book.Select(a => vt.GetTitleViewModel(a, db)).OrderBy(x => x.Title).ToList();
                return View(bookView.ToPagedList<TitleViewModel>(currentPage, onPage));
            }
            if (save != null)
            {
                string url = Request.UrlReferrer.ToString();
                string userId = User.Identity.GetUserId();
                vt.SaveSearch(userId, url, db);
                return Redirect(url);
            }


            var books = db.BookItems.GroupBy(x => x.Title).ToList();
            var booksView = books.Select(a => vt.GetTitleViewModel(a, db)).OrderBy(x => x.Title).ToList();

            return View(booksView.ToPagedList<TitleViewModel>(currentPage, onPage));
        }

        //Get: Isbn
        public ActionResult Isbn(int? id, int? page, string searchString, string submit, bool? save)
        {
            int currentPage = page ?? 1;
            int onPage = 5;

            if (!string.IsNullOrEmpty(submit) && !string.IsNullOrEmpty(searchString))
            {
                var book =
                        db.BookItems.Where(x => x.ISBN.ToString().Contains(searchString)).ToList();
                var bookView = book.Select(a => vt.GetBookViewModel(a, db)).OrderBy(x => x.ISBN).ToList();
                return View(bookView.ToPagedList<BookItemViewModel>(currentPage, onPage));
            }
            if (save != null)
            {
                string url = Request.UrlReferrer.ToString();
                string userId = User.Identity.GetUserId();
                vt.SaveSearch(userId, url, db);
                return Redirect(url);
            }

            var books = db.BookItems.ToList();
            var booksView = books.Select(a => vt.GetBookViewModel(a, db)).OrderBy(x => x.ISBN).ToList();

            return View(booksView.ToPagedList<BookItemViewModel>(currentPage, onPage));
        }

        public ActionResult Label(int? page, string searchString, string submit, bool? save)
        {
            int currentPage = page ?? 1;
            int onPage = 5;

            if (!string.IsNullOrEmpty(submit) && !string.IsNullOrEmpty(searchString))
            {
                var label = db.Labels.Where(x => (x.Name.ToLower().Contains(searchString.ToLower()) && x.LabelGroups.Any())).ToList();
                var labelSearchView = label.Select(a => vt.GetLabelViewModel(a, db)).OrderBy(x => x.Name).ToList();
                return View(labelSearchView.ToPagedList<LabelViewModel>(currentPage, onPage));
            }
            if (save != null)
            {
                string url = Request.UrlReferrer.ToString();
                string userId = User.Identity.GetUserId();
                vt.SaveSearch(userId, url, db);
                return Redirect(url);
            }

            var labels = db.Labels.Where(x => x.LabelGroups.Any()).ToList();
            var labelView = labels.Select(a => vt.GetLabelViewModel(a, db)).OrderBy(x => x.Name).ToList();

            return View(labelView.ToPagedList<LabelViewModel>(currentPage, onPage));
        }

        public ActionResult Category()
        {
            var categories = vt.GetCategoryList_WithChildren(db);
            var js = new JavaScriptSerializer().Serialize(categories);

            ViewBag.Js = js;

            return View();
        }

        public ActionResult CategorySearch(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int currentPage = 1;
            int onPage = 5;

            var booksItems = db.BookItems.Where(a => a.CategoryID == id).ToList();
            var booksView = booksItems.Select(b => vt.GetBookViewModel(b, db)).ToList();

            if (!booksView.Any())
            {
                return HttpNotFound();
            }
            ViewBag.Category = db.Authors.Find(id).FullName;

            return View("Books", booksView.ToPagedList<BookItemViewModel>(currentPage, onPage));
        }

        //Get: Books
        public ActionResult BooksAuthor(int? id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int currentPage = page ?? 1;
            int onPage = 5;

            var booksItems = db.BookItems.Where(a => a.AuthorGroups.Any(x => x.Author.AuthorID == id)).ToList();
            var booksView = booksItems.Select(b => vt.GetBookViewModel(b, db)).ToList();

            if (!booksView.Any())
            {
                return HttpNotFound();
            }
            ViewBag.Category = db.Authors.Find(id).FullName;

            return View("Books", booksView.ToPagedList<BookItemViewModel>(currentPage, onPage));
        }

        //Get: Books
        public ActionResult BooksLabel(int? id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int currentPage = page ?? 1;
            int onPage = 5;

            var booksItems = db.BookItems.Where(a => a.LabelGroups.Any(x => x.Label.LabelID == id)).ToList();
            var booksView = booksItems.Select(b => vt.GetBookViewModel(b, db)).ToList();

            if (!booksView.Any())
            {
                return HttpNotFound();
            }
            ViewBag.Category = db.Labels.Find(id).Name;

            return View("Books", booksView.ToPagedList<BookItemViewModel>(currentPage, onPage));
        }

        //Get: Books
        public ActionResult BooksTitle(int? page, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int currentPage = page ?? 1;
            int onPage = 5;

            var booksItems = db.BookItems.Where(a => a.Title.Equals(id)).ToList();
            var booksView = booksItems.Select(b => vt.GetBookViewModel(b, db)).ToList();

            if (!booksView.Any())
            {
                return HttpNotFound();
            }
            ViewBag.Category = id;

            return View("Books", booksView.ToPagedList<BookItemViewModel>(currentPage, onPage));
        }


    }
}