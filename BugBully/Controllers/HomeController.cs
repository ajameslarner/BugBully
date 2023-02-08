using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugBully.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Bugs()
        {
            return RedirectToAction("Bugs");
        }

        public ActionResult Users()
        {
            return RedirectToAction("Users");
        }

        public ActionResult API()
        {
            return RedirectToAction("BugsAPI");
        }
    }
}