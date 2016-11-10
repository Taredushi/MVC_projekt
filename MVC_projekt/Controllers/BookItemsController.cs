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

namespace MVC_projekt.Controllers
{
    public class BookItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BookItems
        public ActionResult Index()
        {
            return View(db.BookItems.ToList());
        }

        // GET: BookItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookItemViewModel bookItem = GetViewModel(db.BookItems.Include(x => x.Category).Single(x => x.BookItemID == id));
            if (bookItem == null)
            {
                return HttpNotFound();
            }
            return View(bookItem);
        }

        // GET: BookItems/Create
        public ActionResult Create()
        {
            var authors = from a in db.Authors
                          select new SelectListItem
                          {
                              Value = a.AuthorID.ToString(),
                              Text = a.Name + " " + a.Surname
                          };

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.Authors = new SelectList(authors, "Value", "Text");
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
                CreateBookItem(bookItem);
                return RedirectToAction("Index");
            }

            var authors = from a in db.Authors
                          select new SelectListItem
                          {
                              Value = a.AuthorID.ToString(),
                              Text = a.Name + " " + a.Surname
                          };

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.Authors = new SelectList(authors, "Value", "Text");
            ViewBag.Labels = new SelectList(db.Labels, "LabelID", "Name");

            return View(bookItem);
        }

        // GET: BookItems/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: BookItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookItemID,Title,ISBN,Descryption,Publisher,ReleaseDate")] BookItem bookItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bookItem);
        }

        // GET: BookItems/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: BookItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookItem bookItem = db.BookItems.Find(id);
            db.BookItems.Remove(bookItem);
            var ag = db.AuthorGroups.Where(x => x.BookItem.BookItemID == id);
            foreach (var arg in ag)
            {
                db.AuthorGroups.Remove(arg);
            }
            db.SaveChanges();
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

        private BookItemViewModel GetViewModel(BookItem book)
        {

            BookItemViewModel bookView = new BookItemViewModel();
            bookView.ID = book.BookItemID;
            bookView.Title = book.Title;
            bookView.ISBN = book.ISBN;
            bookView.Publisher = book.Publisher;
            bookView.ReleaseDate = book.ReleaseDate;
            bookView.Descryption = book.Descryption;
            bookView.Category = db.Categories.Single(c => c.CategoryID == book.Category.CategoryID);
            bookView.Authors = db.Authors.Where(a => a.AuthorGroups.Any(g => g.BookItem.BookItemID == book.BookItemID)).ToList();
            bookView.Labels = db.Labels.Where(a => a.LabelGroups.Any(g => g.BookItem.BookItemID == book.BookItemID)).ToList();
            bookView.Amount = book.Amount;

            return bookView;
        }

        private void CreateBookItem(BookItemViewModel bookView)
        {
            BookItem bookItem = new BookItem()
            {
                Title = bookView.Title,
                ISBN = bookView.ISBN,
                Descryption = bookView.Descryption,
                Publisher = bookView.Publisher,
                ReleaseDate = bookView.ReleaseDate,
                Category = db.Categories.Find(bookView.CategoryID),
                Amount = bookView.Amount
            };

            db.Set<BookItem>().AddOrUpdate(bookItem);
            db.SaveChanges();

            var book = db.BookItems.FirstOrDefault(x => x.ISBN == bookItem.ISBN);
            db.Categories.Find(bookView.CategoryID).BookItem.Add(book);

            foreach (var authorId in bookView.SelectedAuthors)
            {
                var author = db.Authors.Single(a => a.AuthorID == authorId);
                var ag = new AuthorGroup()
                {
                    Author = author,
                    BookItem = db.BookItems.Find(book.BookItemID),
                };
                db.Set<AuthorGroup>().AddOrUpdate(ag);

                var group = db.AuthorGroups.FirstOrDefault(a => a.Author.AuthorID == authorId && a.BookItem.BookItemID == book.BookItemID);

                author.AuthorGroups.Add(group);
                book.AuthorGroups.Add(group);
            }
            db.SaveChanges();

            foreach (var labelId in bookView.SelectedLabels)
            {
                var label = db.Labels.Single(l => l.LabelID == labelId);
                var labelgroup = new LabelGroup()
                {
                    Label = label,
                    BookItem = db.BookItems.Find(book.BookItemID),
                };
                db.Set<LabelGroup>().AddOrUpdate(labelgroup);

                var group = db.LabelGroups.FirstOrDefault(l => l.Label.LabelID == labelId && l.BookItem.BookItemID == book.BookItemID);

                label.LabelGroups.Add(group);
                book.LabelGroups.Add(group);
            }
            db.SaveChanges();
        }

        #endregion
    }
}
