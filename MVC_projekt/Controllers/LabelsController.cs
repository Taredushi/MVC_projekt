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
    public class LabelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Labels
        public ActionResult Index()
        {
            return View(db.Labels.ToList());
        }

        // GET: Labels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Label label = db.Labels.Find(id);
            if (label == null)
            {
                return HttpNotFound();
            }
            return View(label);
        }

        // GET: Labels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Labels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LabelID,Name")] Label label)
        {
            if (ModelState.IsValid)
            {
                db.Labels.Add(label);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(label);
        }

        // GET: Labels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Label label = db.Labels.Find(id);
            if (label == null)
            {
                return HttpNotFound();
            }
            return View(label);
        }

        // POST: Labels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LabelID,Name")] Label label)
        {
            if (ModelState.IsValid)
            {
                db.Entry(label).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(label);
        }

        // GET: Labels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Label label = db.Labels.Find(id);
            if (label == null)
            {
                return HttpNotFound();
            }
            return View(label);
        }

        // POST: Labels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Label label = db.Labels.Find(id);
            db.Labels.Remove(label);
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
