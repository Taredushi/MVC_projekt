using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Microsoft.AspNet.Identity;
using MVC_projekt.Models;
using MVC_projekt.Models.Helpers;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace MVC_projekt.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        // GET: Cart
        
        public ActionResult Index(int? page, bool? message)
        {
            int currentPage = page ?? 1;
            int onPage = 5;

            var bookItemList = (List<BookItem>)Session["cart"];
            bookItemList = bookItemList ?? new List<BookItem>();

            ViewBag.Message = message;
            return View(bookItemList.ToPagedList<BookItem>(currentPage, onPage));
        }

        public ActionResult Remove(int? id)
        {
            if (id != null)
            {
                CartTools ct = new CartTools();
                ct.RemoveFromCart(this.HttpContext, (int)id);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Save()
        {
            CartTools ct = new CartTools();
            ct.Order(this.HttpContext);

            return RedirectToAction("Index", true);
        }
    }
}