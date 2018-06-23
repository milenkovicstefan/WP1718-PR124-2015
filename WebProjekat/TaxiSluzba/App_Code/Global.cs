using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaxiSluzba.Models;

namespace TaxiSluzba.App_Code
{
    public static class Global
    {
        private static Dictionary<string, Korisnik> korisnici = new Dictionary<string, Korisnik>();

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
    }
}