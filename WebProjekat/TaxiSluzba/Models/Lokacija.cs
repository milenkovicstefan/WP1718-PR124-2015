using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Lokacija
    {
        private double koordinataX;
        private double koordinataY;
        private Adresa adresa;

        #region Properties
        public double KoordinataX
        {
            get
            {
                return koordinataX;
            }

            set
            {
                koordinataX = value;
            }
        }

        public double KoordinataY
        {
            get
            {
                return koordinataY;
            }

            set
            {
                koordinataY = value;
            }
        }

        public Adresa Adresa
        {
            get
            {
                return adresa;
            }

            set
            {
                adresa = value;
            }
        }
        #endregion
    }
}