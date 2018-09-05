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
                        Korisnik korisnik = JsonConvert.DeserializeObject<Korisnik>(s);
                        Global.Dispeceri.Add(korisnik.KorisnickoIme, korisnik);
                        s = "";
                    }
                }
            }
        }
    }
}
