 using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
 using System.Web.Routing;
 using MVC_projekt.Classes;

namespace MVC_projekt.Controllers
{
    [Localization("pl")]
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            var url = Request.RawUrl.Replace("/", "");
            HttpCookie cookie = new HttpCookie("Language");

            cookie.Value = string.IsNullOrEmpty(url) ? "pl" : url;

            Response.SetCookie(cookie);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}