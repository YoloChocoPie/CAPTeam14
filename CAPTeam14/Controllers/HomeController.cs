using CAPTeam14.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CAPTeam14.Controllers
{
    public class HomeController : Controller
    {
        [LoginVerification]
        public ActionResult Index()
        {
            return View();
        }
        [LoginVerification]

        public ActionResult Courses()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [LoginVerification]

        public ActionResult Catalog()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [LoginVerification]

        public ActionResult Search()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [LoginVerification]

        public ActionResult Table()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}