using CAPTeam14.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CAPTeam14.Models;

namespace CAPTeam14.Controllers
{
    [LoginVerification]
    public class HomeController : Controller
    {
        CapTeam14Entities model = new CapTeam14Entities();
        
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult Courses()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        
        public ActionResult Catalog()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
      
        public ActionResult Search()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
    }
}