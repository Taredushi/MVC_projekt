using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using MVC_projekt.Models;
using MVC_projekt.Models.Helpers;
using PagedList;

namespace MVC_projekt.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ViewTools vt = new ViewTools();

        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        // GET: BookItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookItemViewModel bookItem = vt.GetViewModel(db.BookItems.Include(x => x.Category).Single(x => x.BookItemID == id), db);

            if (bookItem == null)
            {
                return HttpNotFound();
            }
            return View(bookItem);
        }

        //Get: Author
        public ActionResult Author(int? page, string searchString, string submit)
        {
            int currentPage = page ?? 1;
            int onPage = 5;

            if (!string.IsNullOrEmpty(submit))
            {
                if (submit.StartsWith("Search") && !string.IsNullOrEmpty(searchString))
                {
                    var author = db.Authors.Where(x => (x.Name.ToLower().Contains(searchString.ToLower()) || x.Surname.ToLower().Contains(searchString.ToLower())) && x.AuthorGroups.Any()).ToList();
                    var authorSearchView = author.Select(a => vt.GetAuthorViewModel(a, db)).OrderBy(x => x.FullName).ToList();
                    return View(authorSearchView.ToPagedList<AuthorViewModel>(currentPage, onPage));
                }
                else if (submit.StartsWith("Save"))
                {
                    //add save search result
                }
            }

            var authors = db.Authors.Where(x=>x.AuthorGroups.Any()).ToList();
            var authorView = authors.Select(a => vt.GetAuthorViewModel(a, db)).OrderBy(x=>x.FullName).ToList();

            return View(authorView.ToPagedList<AuthorViewModel>(currentPage, onPage));
        }

        //Get: Title
        public ActionResult Title(int? page, string searchString, string submit)
        {
            int currentPage = page ?? 1;
            int onPage = 5;

            if (!string.IsNullOrEmpty(submit))
            {
                if (submit.StartsWith("Search") && !string.IsNullOrEmpty(searchString))
                {
                    var book =
                        db.BookItems.Where(x => x.Title.ToUpper().Contains(searchString.ToUpper()))
                            .GroupBy(x => x.Title)
                            .ToList();
                    var bookView = book.Select(a => vt.GetTitleViewModel(a, db)).OrderBy(x => x.Title).ToList();
                    return View(bookView.ToPagedList<TitleViewModel>(currentPage, onPage));
                }
                else if (submit.StartsWith("Save"))
                {
                    //add save search result
                }
            }

            var books = db.BookItems.GroupBy(x=>x.Title).ToList();
            var booksView = books.Select(a => vt.GetTitleViewModel(a, db)).OrderBy(x => x.Title).ToList();

            return View(booksView.ToPagedList<TitleViewModel>(currentPage, onPage));
        }

        //Get: Isbn
        public ActionResult Isbn(int? id, int? page, string searchString, string submit)
        {
            int currentPage = page ?? 1;
            int onPage = 5;

            if (!string.IsNullOrEmpty(submit))
            {
                if (submit.StartsWith("Search") && !string.IsNullOrEmpty(searchString))
                {
                    var book =
                        db.BookItems.Where(x => x.ISBN.ToString().Contains(searchString)).ToList();
                    var bookView = book.Select(a => vt.GetBookViewModel(a, db)).OrderBy(x => x.ISBN).ToList();
                    return View(bookView.ToPagedList<BookItemViewModel>(currentPage, onPage));
                }
                else if (submit.StartsWith("Save"))
                {
                    //add save search result
                }
            }

            var books = db.BookItems.ToList();
            var booksView = books.Select(a => vt.GetBookViewModel(a, db)).OrderBy(x => x.ISBN).ToList();

            return View(booksView.ToPagedList<BookItemViewModel>(currentPage, onPage));
        }


        //Get: Books
        [ActionName("BooksAuthor")]
        public ActionResult Books(int? id, int? page)
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
        [ActionName("BooksTitle")]
        public ActionResult Books(int? page, string id)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Helpers


        #endregion
    }
}