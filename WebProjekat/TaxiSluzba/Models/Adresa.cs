using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Adresa
    {
        private string ulica;
        private int broj;
        private string mesto;
        private int postanskiBroj;

        #region Properties
        public string Ulica
        {
            get
            {
                return ulica;
            }

            set
            {
                ulica = value;
            }
        }

        public int Broj
        {
            get
            {
                return broj;
            }

            set
            {
                broj = value;
            }
        }

        public string Mesto
        {
            get
            {
                return mesto;
            }

            set
            {
                mesto = value;
            }
        }

        public int PostanskiBroj
        {
            get
            {
                return postanskiBroj;
            }

            set
            {
                postanskiBroj = value;
            }
        }
        #endregion
    }
}