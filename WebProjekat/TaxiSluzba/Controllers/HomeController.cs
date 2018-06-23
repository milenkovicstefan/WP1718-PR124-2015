using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiSluzba.Models;

namespace TaxiSluzba.Controllers
{
    public class HomeController : Controller
    {
        private static Dictionary<string, Korisnik> korisnici = new Dictionary<string, Korisnik>();
        public ActionResult Index()
        {
            string path = Server.MapPath("~/App_Data/Dispeceri.txt");
            //Korisnik k = new Korisnik()
            //{
            //    KorisnickoIme = "kime",
            //    Ime = "ime",
            //    Prezime = "prezime",
            //    Jmbg = "2204996762035",
            //    Email = "example@mail.com",
            //    KontaktTelefon = "0666266230",
            //    Lozinka = "lozinka",
            //    Pol = Pol.MUSKI,
            //    Uloga = Uloga.DISPECER
            //};
            //if (!System.IO.File.Exists(path))
            //{
            //    // Create a file to write to.
            //    using (StreamWriter sw = System.IO.File.CreateText(path))
            //    {
            //        sw.WriteLine(JsonConvert.SerializeObject(k, Formatting.Indented));
            //    }
            //}
            //else
            //{
            //    using (StreamWriter sw = System.IO.File.AppendText(path))
            //    {
            //        sw.WriteLine(JsonConvert.SerializeObject(k, Formatting.Indented));
            //    }
            //}

            ViewBag.Title = "Taxi služba";

            return View(korisnici);
        }
    }
}
