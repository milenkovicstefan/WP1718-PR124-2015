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

            using (var sr = new StreamReader(path))
            {
                string s = "";
                string nextLine = "";
                while ((nextLine = sr.ReadLine()) != null)
                {
                    s += nextLine;
                    if (nextLine.Contains("}"))
                    {
                        Dispecer dispecer = JsonConvert.DeserializeObject<Dispecer>(s);
                        Global.Dispeceri.Add(dispecer.KorisnickoIme, dispecer);
                        s = "";
                    }
                }
            }
        }

        protected void UcitajVozace()
        {
            string path = Server.MapPath("~/App_Data/Vozaci.txt");

            using (var sr = new StreamReader(path))
            {
                string s = "";
                string nextLine = "";
                while ((nextLine = sr.ReadLine()) != null)
                {
                    s += nextLine;
                    if (nextLine.Contains("}"))
                    {
                        Vozac vozac = JsonConvert.DeserializeObject<Vozac>(s);
                        Global.Vozaci.Add(vozac.KorisnickoIme, vozac);
                        s = "";
                    }
                }
            }
        }

        protected void UcitajMusterije()
        {
            string path = Server.MapPath("~/App_Data/Musterije.txt");

            using (var sr = new StreamReader(path))
            {
                string s = "";
                string nextLine = "";
                while ((nextLine = sr.ReadLine()) != null)
                {
                    s += nextLine;
                    if (nextLine.Contains("}"))
                    {
                        Musterija musterija = JsonConvert.DeserializeObject<Musterija>(s);
                        Global.Musterije.Add(musterija.KorisnickoIme, musterija);
                        s = "";
                    }
                }
            }
        }

        protected void UcitajVoznje()
        {
            string path = Server.MapPath("~/App_Data/Voznje.txt");

            using (var sr = new StreamReader(path))
            {
                string s = "";
                string nextLine = "";
                while ((nextLine = sr.ReadLine()) != null)
                {
                    s += nextLine;
                    if (nextLine.Contains("}"))
                    {
                        Voznja voznja = JsonConvert.DeserializeObject<Voznja>(s);
                        Global.Voznje.Add(voznja.VremePorudzbine.ToString(), voznja);
                        s = "";
                    }
                }
            }
        }


    }
}
