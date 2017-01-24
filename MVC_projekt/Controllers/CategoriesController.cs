using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_projekt.Models;
using MVC_projekt.Models.View;

namespace MVC_projekt.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index()
        {
            return View(db.Categories.Where(c => c.CategoryID != -1).ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            category.Parent = db.Categories.Find(category.ParentID);
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.OverCategories = new SelectList(db.Categories, "CategoryID", "Name");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryViewModelCreate categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category();
                if (categoryViewModel.ParentID != null)
                {
                    category.ParentID = categoryViewModel.ParentID.Value;
                    category.Parent = db.Categories.FirstOrDefault(x => x.CategoryID == category.ParentID);
                }
                category.Name = categoryViewModel.Name;
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoryViewModel);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            CategoryViewModelCreate categoryViewModel = new CategoryViewModelCreate();
            categoryViewModel.CategoryID = category.CategoryID;
            categoryViewModel.Name = category.Name;
            categoryViewModel.ParentID = category.ParentID;
            ViewBag.OverCategories = new SelectList(db.Categories.Where(x => x.CategoryID != category.CategoryID),
                "CategoryID", "Name");
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(categoryViewModel);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryViewModelCreate categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var category = db.Categories.FirstOrDefault(x => x.CategoryID == categoryViewModel.CategoryID);
                if (categoryViewModel.ParentID != null)
                {
                    category.ParentID = categoryViewModel.ParentID.Value;
                    category.Parent = db.Categories.FirstOrDefault(x => x.CategoryID == category.ParentID);
                }
                category.Name = categoryViewModel.Name;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoryViewModel);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            int overcategory;
            if (category.ParentID == 0)
            {
                overcategory = -1;
            }
            else
            {
                overcategory = category.ParentID;
            }
            category.ParentID = 0;
            category.Parent = null;

            foreach (var book in db.BookItems.ToList())
            {
                if (book.CategoryID == category.CategoryID)
                {
                    book.Category = null;
                    book.CategoryID = overcategory;
                    var newcategory = db.Categories.FirstOrDefault(c => c.CategoryID == overcategory);
                    book.Category = newcategory;
                    db.SaveChanges();
                }
            }

            foreach (var dbcategory in db.Categories.ToList())
            {
                if (dbcategory.ParentID == category.CategoryID)
                {
                    dbcategory.Parent = null;
                    dbcategory.ParentID = 0;
                    db.SaveChanges();
                }
            }
            db.SaveChanges();
            db.Categories.Remove(category);
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