using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Komentar
    {
        private string opis;
        private DateTime datumObjave;
        private Korisnik vlasnikKomentara;
        private Voznja komentarisanaVoznja;
        private Ocena ocena;

        #region Properties
        public string Opis
        {
            get
            {
                return opis;
            }

            set
            {
                opis = value;
            }
        }

        public DateTime DatumObjave
        {
            get
            {
                return datumObjave;
            }

            set
            {
                datumObjave = value;
            }
        }

        public Korisnik VlasnikKomentara
        {
            get
            {
                return vlasnikKomentara;
            }

            set
            {
                vlasnikKomentara = value;
            }
        }

        public Voznja KomentarisanaVoznja
        {
            get
            {
                return komentarisanaVoznja;
            }

            set
            {
                komentarisanaVoznja = value;
            }
        }

        public Ocena Ocena
        {
            get
            {
                return ocena;
            }

            set
            {
                ocena = value;
            }
        }
        #endregion
    }
}