using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using TaxiSluzba.Models;

namespace TaxiSluzba.App_Code
{
    public static class Global
    {
        private static Dictionary<string, Dispecer> dispeceri = new Dictionary<string, Dispecer>();
        private static Dictionary<string, Korisnik> korisnici = new Dictionary<string, Korisnik>();
        private static Dictionary<string, Vozac> vozaci = new Dictionary<string, Vozac>();
        private static Dictionary<string, Musterija> musterije = new Dictionary<string, Musterija>();
        private static Dictionary<string, Korisnik> blokirani = new Dictionary<string, Korisnik>();
        private static Dictionary<string, Voznja> voznje = new Dictionary<string, Voznja>();
        private static Dictionary<string, Automobil> automobili = new Dictionary<string, Automobil>();

        public static Dictionary<string, Dispecer> Dispeceri
        {
            get
            {
                return dispeceri;
            }

            set
            {
                dispeceri = value;
            }
        }

        public static Dictionary<string, Korisnik> Korisnici
        {
            get
            {
                return korisnici;
            }

            set
            {
                korisnici = value;
            }
        }

        public static Dictionary<string, Korisnik> Blokirani
        {
            get
            {
                return blokirani;
            }

            set
            {
                blokirani = value;
            }
        }

        public static Dictionary<string, Vozac> Vozaci
        {
            get
            {
                return vozaci;
            }

            set
            {
                vozaci = value;
            }
        }

        public static Dictionary<string, Musterija> Musterije
        {
            get
            {
                return musterije;
            }

            set
            {
                musterije = value;
            }
        }

        public static Dictionary<string, Voznja> Voznje
        {
            get
            {
                return voznje;
            }

            set
            {
                voznje = value;
            }
        }

        public static Dictionary<string, Automobil> Automobili
        {
            get
            {
                return automobili;
            }

            set
            {
                automobili = value;
            }
        }

        public static void RewriteAutomobiliTxt()
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Automobili.txt");
            List<string> objects = new List<string>();
            foreach (var item in Automobili.Values)
            {
                string obj = JsonConvert.SerializeObject(item, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                });
                obj += "###";
                objects.Add(obj);
            }
            File.WriteAllLines(path, objects);
        }

        public static void RewriteBanovaniTxt()
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Banovani.txt");
            List<string> objects = new List<string>();
            foreach (var item in Blokirani.Values)
            {
                string obj = JsonConvert.SerializeObject(item, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                });
                obj += "###";
                objects.Add(obj);
            }
            File.WriteAllLines(path, objects);
        }

        public static void RewriteDispeceriTxt()
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Dispeceri.txt");
            List<string> objects = new List<string>();
            foreach (var item in Dispeceri.Values)
            {
                string obj = JsonConvert.SerializeObject(item, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                });
                obj += "###";
                objects.Add(obj);
            }
            File.WriteAllLines(path, objects);
        }

        public static void RewriteMusterijeTxt()
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Musterije.txt");
            List<string> objects = new List<string>();
            foreach (var item in Musterije.Values)
            {
                string obj = JsonConvert.SerializeObject(item, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                });
                obj += "###";
                objects.Add(obj);
            }
            File.WriteAllLines(path, objects);
        }

        public static void RewriteVozaciTxt()
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Vozaci.txt");
            List<string> objects = new List<string>();
            foreach (var item in Vozaci.Values)
            {
                string obj = JsonConvert.SerializeObject(item, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                });
                obj += "###";
                objects.Add(obj);
            }
            File.WriteAllLines(path, objects);
        }

        public static void RewriteVoznjeTxt()
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Voznje.txt");
            List<string> objects = new List<string>();
            foreach (var item in Voznje.Values)
            {
                string obj = JsonConvert.SerializeObject(item, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                });
                obj += "###";
                objects.Add(obj);
            }
            File.WriteAllLines(path, objects);
        }

        public static void RewriteAllTxt()
        {
            RewriteDispeceriTxt();
            RewriteMusterijeTxt();
            RewriteVozaciTxt();
            RewriteAutomobiliTxt();
            RewriteVoznjeTxt();
        }
    }
}