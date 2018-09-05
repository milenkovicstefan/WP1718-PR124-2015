using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiSluzba.App_Code;
using TaxiSluzba.Models;

namespace TaxiSluzba.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //string path = Server.MapPath("~/App_Data/Dispeceri.txt");
            
            ViewBag.Title = "Taxi služba";

            return View(Global.Dispeceri);
        }

        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult Driver()
        {
            return View();
        }

        public ActionResult Customer()
        {
            return View();
        }

        public ActionResult EditProfile()
        {
            return View();
        }
    }
}
