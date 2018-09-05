using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Vozac : Korisnik
    {
        private Lokacija lokacija;
        private Automobil automobil;

        #region Properties
        public Lokacija Lokacija
        {
            get
            {
                return lokacija;
            }

            set
            {
                lokacija = value;
            }
        }

        public Automobil Automobil
        {
            get
            {
                return automobil;
            }

            set
            {
                automobil = value;
            }
        }
        #endregion
    }
}