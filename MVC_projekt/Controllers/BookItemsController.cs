using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVC_projekt.Models;
using MVC_projekt.Models.Classes;
using MVC_projekt.Models.Helpers;
using MVC_projekt.Models.View;
using PagedList;

namespace MVC_projekt.Controllers
{
    [Localization("pl")]
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
            var av = bookItem.Number - db.Orders.Count(o => o.BookItemID == bookItem.BookItemID && !o.Returned);
            ViewBag.Available = av;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookItemViewModel bookItem, HttpPostedFileBase cover, HttpPostedFileBase tableOfContents)
        {
            var allowedExtensions = new[] {".jpg", ".png", ".bmp"};
            if (!allowedExtensions.Contains(Path.GetExtension(tableOfContents.FileName)))
            {
                ModelState.AddModelError("Extension", @"Wrong table of contents extension");
                ViewBag.TableOfContent = true;
            }
            if (ModelState.IsValid)
            {
                if (!db.BookItems.Any(b => b.ISBN == bookItem.ISBN))
                {
                    vt.CreateBookItem(bookItem, db, cover, tableOfContents);
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
            BookItemViewModel bookItem = vt.GetBookViewModel(db.BookItems.Include(x => x.Category).Single(x => x.BookItemID == id), db);
            

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
                var directory = Path.Combine(Path.Combine("/Upload", id.ToString()));
                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory);
                }

                db.SaveChanges();
            }
            catch (Exception)
            {
                
                return RedirectToAction("Delete", new {id = id, error = true});
            }
            
            return RedirectToAction("Index");
        }


        public FileResult Download(string filename, string bookID, string source)
        {
            var directory = Path.Combine(Path.Combine("/Upload", bookID));
            var filepath = Path.Combine(directory, source);
            var contentType = "application/" + Path.GetExtension(filepath);
            return File(filepath, contentType, filename);
        }

        #region Order
        [HttpPost]
        public ActionResult Order(int id)
        {
            CartTools ct = new CartTools();
            ct.AddToCart(this.HttpContext, id);

            return RedirectToAction("Details", new {id});
        }

        [HttpPost]
        public ActionResult ReturnOrder(int id)
        {
            CartTools ct = new CartTools();
            ct.ReturnOrder(id);

            return RedirectToAction("Index");
        }
        #endregion

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
