using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_projekt.Models;
using MVC_projekt.Models.Helpers;
using PagedList;

namespace MVC_projekt.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index(int? id, int? page)
        {
            int currentPage = page ?? 1;
            int onPage = 5;

            if (id != null)
            {
                var orderList = db.Orders.Where(x => x.BookItemID == id && !x.Returned).ToList();

                var book = db.BookItems.Single(x => x.BookItemID == id);
                string author = book.AuthorGroups.Aggregate("", (current, arg) => current + (arg.Author.FullName + " "));
                ViewBag.Title = author + " / " + book.Title;
                ViewBag.Max = book.Number;
                ViewBag.ID = id;
                return View(orderList.ToPagedList<Order>(currentPage, onPage));
            }

            return View();
        }

        public ActionResult Order(int? id)
        {
            if (id != null)
            {
                CartTools ct = new CartTools();
                ct.AddToCart(this.HttpContext, (int)id);
            }

            return RedirectToAction("Details", "BookItems", new {id});
        }
    }
}