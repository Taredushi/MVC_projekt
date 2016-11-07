using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
            var bookItems = db.BookItems.Include(b => b.Category);
            return View(bookItems.ToList());
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

        // GET: BookItems/Create
        public ActionResult Create()
        {
            ViewBag.BookItemID = new SelectList(db.Categories, "CategoryID", "Name");
            return View();
        }

        // POST: BookItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookItemID,Title,ISBN,Descryption,Publisher,ReleaseDate")] BookItem bookItem)
        {
            if (ModelState.IsValid)
            {
                db.BookItems.Add(bookItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookItemID = new SelectList(db.Categories, "CategoryID", "Name", bookItem.BookItemID);
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
            ViewBag.BookItemID = new SelectList(db.Categories, "CategoryID", "Name", bookItem.BookItemID);
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
            ViewBag.BookItemID = new SelectList(db.Categories, "CategoryID", "Name", bookItem.BookItemID);
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
    }
}
