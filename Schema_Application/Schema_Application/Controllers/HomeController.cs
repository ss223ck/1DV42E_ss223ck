using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Schema_Application.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Schema Application";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Sverker";

            return View();
        }
    }
}