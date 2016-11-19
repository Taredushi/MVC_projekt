using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_projekt.Models;
using MVC_projekt.Models.Helpers;
using PagedList;

namespace MVC_projekt.Controllers
{
    public class BookItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ViewTools vt = new ViewTools();

        // GET: BookItems
        public ActionResult Index(int? page)
        {
            int currentPage = page ?? 1;
            int onPage = 5;

            List<BookItem> bookItemList = db.BookItems.OrderBy(x=>x.BookItemID).ToList();

            return View(bookItemList.ToPagedList<BookItem>(currentPage, onPage));
        }

        // GET: BookItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookItem bookItem = db.BookItems.Find(id);

            if (bookItem == null)
            {
                return HttpNotFound();
            }
            return View(bookItem);
        }

        [Authorize(Roles = "Admin, Employee")]
        // GET: BookItems/Create
        public ActionResult Create()
        {

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.Authors = new SelectList(vt.GetAuthorsFromDb(db), "Value", "Text");
            ViewBag.Labels = new SelectList(db.Labels, "LabelID", "Name");
            return View();
        }

        // POST: BookItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookItemViewModel bookItem)
        {
            if (ModelState.IsValid)
            {
                if (!db.BookItems.Any(b => b.ISBN == bookItem.ISBN))
                {
                    vt.CreateBookItem(bookItem, db);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = true;
                    ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
                    ViewBag.Authors = new SelectList(vt.GetAuthorsFromDb(db), "Value", "Text");
                    ViewBag.Labels = new SelectList(db.Labels, "LabelID", "Name");

                    return View(bookItem);
                }
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.Authors = new SelectList(vt.GetAuthorsFromDb(db), "Value", "Text");
            ViewBag.Labels = new SelectList(db.Labels, "LabelID", "Name");

            return View(bookItem);
        }

        [Authorize(Roles = "Admin, Employee")]
        // GET: BookItems/Edit/5
        public ActionResult Edit(int? id)
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

            bookItem.PreviousPage = Request.UrlReferrer.ToString();
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.Authors = new SelectList(vt.GetAuthorsFromDb(db), "Value", "Text");
            ViewBag.Labels = new SelectList(db.Labels, "LabelID", "Name");
            ViewBag.Init = true;

            return View(bookItem);
        }

        // POST: BookItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookItemViewModel bookView)
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
            
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.Authors = new SelectList(vt.GetAuthorsFromDb(db), "Value", "Text");
            ViewBag.Labels = new SelectList(db.Labels, "LabelID", "Name");
            ViewBag.Error = false;
            ViewBag.Init = false;
            return View(bookView);
        }

        [Authorize(Roles = "Admin, Employee")]
        // GET: BookItems/Delete/5
        public ActionResult Delete(int? id, bool? error)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookItem bookItem = db.BookItems.Find(id);

            if (bookItem == null)
            {
                return HttpNotFound();
            }

            if (error != null)
            {
                ViewBag.Error = true;
            }

            return View(bookItem);
        }

        // POST: BookItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                BookItem bookItem = db.BookItems.Single(x => x.BookItemID == id);
                db.BookItems.Remove(bookItem);
                db.SaveChanges();
            }
            catch (Exception)
            {
                
                return RedirectToAction("Delete", new {id = id, error = true});
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

        #region Search

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

            var authors = db.Authors.Where(x => x.AuthorGroups.Any()).ToList();
            var authorView = authors.Select(a => vt.GetAuthorViewModel(a, db)).OrderBy(x => x.FullName).ToList();

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

            var books = db.BookItems.GroupBy(x => x.Title).ToList();
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

        public ActionResult Label(int? page, string searchString, string submit)
        {
            int currentPage = page ?? 1;
            int onPage = 5;

            if (!string.IsNullOrEmpty(submit))
            {
                if (submit.StartsWith("Search") && !string.IsNullOrEmpty(searchString))
                {
                    var label = db.Labels.Where(x => (x.Name.ToLower().Contains(searchString.ToLower()) && x.LabelGroups.Any())).ToList();
                    var labelSearchView = label.Select(a => vt.GetLabelViewModel(a, db)).OrderBy(x => x.Name).ToList();
                    return View(labelSearchView.ToPagedList<LabelViewModel>(currentPage, onPage));
                }
                else if (submit.StartsWith("Save"))
                {
                    //add save search result
                }
            }

            var labels = db.Labels.Where(x => x.LabelGroups.Any()).ToList();
            var labelView = labels.Select(a => vt.GetLabelViewModel(a, db)).OrderBy(x => x.Name).ToList();

            return View(labelView.ToPagedList<LabelViewModel>(currentPage, onPage));
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

        #endregion
    }
}
