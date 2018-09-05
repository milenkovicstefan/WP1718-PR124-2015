using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    }
}