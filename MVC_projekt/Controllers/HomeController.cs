 using System;
 using System.Collections;
 using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
 using System.Web.Routing;
 using MVC_projekt.Classes;
 using MVC_projekt.Models;

namespace MVC_projekt.Controllers
{
    [Localization("pl")]
    public class HomeController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var url = Request.RawUrl.Replace("/", "");
            url = string.IsNullOrEmpty(url) ? "pl" : url;

            if (Session["Language"] == null)
            {
                Session.Add("Language", url);
            }
            else
            {
                Session["Language"] = url;
            }

            var latestBooks = db.BookItems.OrderByDescending(d => d.AddDate).Take(3).ToList();

            return View(latestBooks);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Find us...";

            return View();
        }

    }
}