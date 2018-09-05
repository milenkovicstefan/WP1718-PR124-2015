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
        private static Dictionary<string, Korisnik> dispeceri = new Dictionary<string, Korisnik>();
        private static Dictionary<string, Korisnik> korisnici = new Dictionary<string, Korisnik>();
        private static Dictionary<string, Korisnik> blokirani = new Dictionary<string, Korisnik>();
        private static List<Voznja> voznje = new List<Voznja>();

        public static Dictionary<string, Korisnik> Dispeceri
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

        public static List<Voznja> Voznje
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