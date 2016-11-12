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

        //Get: Author
        public ActionResult Author(int? page, string searchString, string submit)
        {
            int currentPage = page ?? 1;
            int onPage = 5;

            if (!string.IsNullOrEmpty(submit))
            {
                if (submit.StartsWith("Search"))
                {
                    var author = db.Authors.Where(x => x.Name.ToLower().Contains(searchString.ToLower()) || x.Surname.ToLower().Contains(searchString.ToLower())).ToList();
                    var authorSearchView = author.Select(a => vt.GetAuthorViewModel(a, db)).OrderBy(x => x.FullName).ToList();
                    return View(authorSearchView.ToPagedList<AuthorViewModel>(currentPage, onPage));
                }
                else if (submit.StartsWith("Save"))
                {
                    //add save search result
                }
            }

            var authors = db.Authors.ToList();
            var authorView = authors.Select(a => vt.GetAuthorViewModel(a, db)).OrderBy(x=>x.FullName).ToList();

            return View(authorView.ToPagedList<AuthorViewModel>(currentPage, onPage));
        }

        //Get: Books
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
            ViewBag.Author = db.Authors.Find(id).FullName;
            ViewBag.AuthorID = id;

            return View("Books", booksView.ToPagedList<BookItemViewModel>(currentPage, onPage));
        }

        // GET: BookItems/Details/5
        public ActionResult Details(int? id, int? authorID)
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

            if (authorID != null)
            {
                ViewBag.AuthorID = authorID;
            }

            return View(bookItem);
        }

        [Authorize(Roles = "Admin, Employee")]
        // GET: BookItems/Edit/5
        public ActionResult Edit(int? id, int? authorID)
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

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.Authors = new SelectList(vt.GetAuthorsFromDb(db), "Value", "Text");
            ViewBag.Labels = new SelectList(db.Labels, "LabelID", "Name");
            ViewBag.Init = true;

            if (authorID != null)
            {
                ViewBag.AuthorID = authorID;
            }

            return View(bookItem);
        }

        // POST: BookItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookItemViewModel bookView, int? authorID)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookItem bookItem = vt.EditBookItem(bookView, db);
                    db.Entry(bookItem).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    throw null;
                }
            }
            catch (Exception)
            {

                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
                ViewBag.Authors = new SelectList(vt.GetAuthorsFromDb(db), "Value", "Text");
                ViewBag.Labels = new SelectList(db.Labels, "LabelID", "Name");

                ViewBag.Init = true;
                ViewBag.Error = true;
                return View(bookView);
            }

            ViewBag.Error = false;

            if (authorID != null)
            {
                ViewBag.AuthorID = authorID;
            }

            ViewBag.Init = false;

            return View(bookView);
        }

        [Authorize(Roles = "Admin, Employee")]
        // GET: BookItems/Delete/5
        public ActionResult Delete(int? id, bool? error, int? authorID)
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

            if (error != null)
            {
                ViewBag.Error = true;
            }
            if (authorID != null)
            {
                ViewBag.AuthorID = authorID;
            }

            return View(bookItem);
        }

        // POST: BookItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? authorID)
        {
            try
            {
                BookItem bookItem = db.BookItems.Single(x => x.BookItemID == id);
                db.BookItems.Remove(bookItem);
                db.SaveChanges();
            }
            catch (Exception)
            {

                return RedirectToAction("Delete", new { id = id, error = true, authorID = authorID });
            }

            return RedirectToAction("Index");
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