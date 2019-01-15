using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TaxiSluzba.Models;
using TaxiSluzba.App_Code;
using System.Web.SessionState;

namespace TaxiSluzba
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UcitajDispecere();
            UcitajMusterije();
            UcitajVozace();
            UcitajVoznje();
            UcitajBanovane();
            var config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling
= Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }

        protected void UcitajDispecere()
        {
            string path = Server.MapPath("~/App_Data/Dispeceri.txt");

            var txt = File.ReadAllText(path);
            var txtLines = txt.Split(new string[] { "###" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (txtLines.Contains(Environment.NewLine))
                txtLines.Remove(Environment.NewLine);
            foreach (var line in txtLines)
            {
                Dispecer dispecer = JsonConvert.DeserializeObject<Dispecer>(line);
                Global.Dispeceri.Add(dispecer.KorisnickoIme, dispecer);
            }

            //using (var sr = new StreamReader(path))
            //{
            //    string s = "";
            //    string nextLine = "";
            //    while ((nextLine = sr.ReadLine()) != null)
            //    {
            //        s += nextLine;
            //        if (nextLine.Contains("}"))
            //        {
            //            Dispecer dispecer = JsonConvert.DeserializeObject<Dispecer>(s);
            //            Global.Dispeceri.Add(dispecer.KorisnickoIme, dispecer);
            //            s = "";
            //        }
            //    }
            //}
        }

        protected void UcitajVozace()
        {
            string path = Server.MapPath("~/App_Data/Vozaci.txt");

            var txt = File.ReadAllText(path);
            var txtLines = txt.Split(new string[] { "###" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (txtLines.Contains(Environment.NewLine))
                txtLines.Remove(Environment.NewLine);
            foreach (var line in txtLines)
            {
                Vozac vozac = JsonConvert.DeserializeObject<Vozac>(line);
                Global.Vozaci.Add(vozac.KorisnickoIme, vozac);
                Global.Korisnici.Add(vozac.KorisnickoIme, (Korisnik)vozac);
            }

            //using (var sr = new StreamReader(path))
            //{
            //    int counter = 0;
            //    string s = "";
            //    string nextLine = "";
            //    while ((nextLine = sr.ReadLine()) != null)
            //    {
            //        s += nextLine;
            //        if (nextLine.Contains("}"))
            //        {
            //            counter++;
            //            if (counter == 5)
            //            {
            //                Vozac vozac = JsonConvert.DeserializeObject<Vozac>(s);
            //                Global.Vozaci.Add(vozac.KorisnickoIme, vozac);
            //                Global.Korisnici.Add(vozac.KorisnickoIme, (Korisnik)vozac);
            //                s = "";
            //                counter = 0;
            //            }
            //        }
            //    }
            //}
        }

        protected void UcitajMusterije()
        {
            string path = Server.MapPath("~/App_Data/Musterije.txt");

            var txt = File.ReadAllText(path);
            var txtLines = txt.Split(new string[] { "###" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (txtLines.Contains(Environment.NewLine))
                txtLines.Remove(Environment.NewLine);
            foreach (var line in txtLines)
            {
                Musterija musterija = JsonConvert.DeserializeObject<Musterija>(line);
                Global.Musterije.Add(musterija.KorisnickoIme, musterija);
                Global.Korisnici.Add(musterija.KorisnickoIme, (Korisnik)musterija);
            }

            //using (var sr = new StreamReader(path))
            //{
            //    string s = "";
            //    string nextLine = "";
            //    while ((nextLine = sr.ReadLine()) != null)
            //    {
            //        s += nextLine;
            //        if (nextLine.Contains("}"))
            //        {
            //            Musterija musterija = JsonConvert.DeserializeObject<Musterija>(s);
            //            Global.Musterije.Add(musterija.KorisnickoIme, musterija);
            //            Global.Korisnici.Add(musterija.KorisnickoIme, (Korisnik)musterija);
            //            s = "";
            //        }
            //    }
            //}
        }

        protected void UcitajVoznje()
        {
            string path = Server.MapPath("~/App_Data/Voznje.txt");

            var txt = File.ReadAllText(path);
            var txtLines = txt.Split(new string[] { "###" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (txtLines.Contains(Environment.NewLine))
                txtLines.Remove(Environment.NewLine);
            foreach (var line in txtLines)
            {
                Voznja voznja = JsonConvert.DeserializeObject<Voznja>(line);
                Global.Voznje.Add(voznja.VremePorudzbine.Ticks.ToString(), voznja);
            }

            //using (var sr = new StreamReader(path))
            //{
            //    int counter = 0;
            //    string s = "";
            //    string nextLine = "";
            //    while ((nextLine = sr.ReadLine()) != null)
            //    {
            //        s += nextLine;
            //        if (nextLine.Contains("}"))
            //        {
            //            counter++;
            //            if (counter == 4)
            //            {
            //                Voznja voznja = JsonConvert.DeserializeObject<Voznja>(s);
            //                Global.Voznje.Add(voznja.VremePorudzbine.Ticks.ToString(), voznja);
            //                s = "";
            //                counter = 0;
            //            }
            //        }
            //    }
            //}
        }

        protected void UcitajBanovane()
        {
            string path = Server.MapPath("~/App_Data/Banovani.txt");

            var txt = File.ReadAllText(path);
            var txtLines = txt.Split(new string[] { "###" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (txtLines.Contains(Environment.NewLine))
                txtLines.Remove(Environment.NewLine);
            foreach (var line in txtLines)
            {
                Korisnik k = JsonConvert.DeserializeObject<Korisnik>(line);
                Global.Blokirani.Add(k.KorisnickoIme, k);
            }
        }
    }
}
