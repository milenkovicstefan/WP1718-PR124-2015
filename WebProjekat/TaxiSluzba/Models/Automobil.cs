using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Automobil
    {
        private Vozac vozac;
        private int godiste;
        private string registarskaOznaka;
        private string taxiOznaka;
        private TipVozila tip;

        #region Properties
        public Vozac Vozac
        {
            get
            {
                return vozac;
            }

            set
            {
                vozac = value;
            }
        }

        public int Godiste
        {
            get
            {
                return godiste;
            }

            set
            {
                godiste = value;
            }
        }

        public string RegistarskaOznaka
        {
            get
            {
                return registarskaOznaka;
            }

            set
            {
                registarskaOznaka = value;
            }
        }

        public string TaxiOznaka
        {
            get
            {
                return taxiOznaka;
            }

            set
            {
                taxiOznaka = value;
            }
        }

        public TipVozila Tip
        {
            get
            {
                return tip;
            }

            set
            {
                tip = value;
            }
        }
        #endregion
    }
}