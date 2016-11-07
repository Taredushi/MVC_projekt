using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace MVC_projekt.Controllers
{
    public class AdminPanelController : Controller
    {
        // GET: AdminPanel
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                ViewBag.Role = "Admin";
            }
            return View();
        }
    }
}