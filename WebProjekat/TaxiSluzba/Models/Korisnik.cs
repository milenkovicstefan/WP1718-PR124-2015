using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Korisnik
    {
        private string korisnickoIme;
        private string lozinka;
        private string ime;
        private string prezime;
        private Pol pol;
        private string jmbg;
        private string kontaktTelefon;
        private string email;
        private Uloga uloga;
        private List<Voznja> voznje;

        public Korisnik()
        {
            Voznje = new List<Voznja>();
        }

        #region Properties
        public string KorisnickoIme
        {
            get
            {
                return korisnickoIme;
            }

            set
            {
                korisnickoIme = value;
            }
        }

        public string Lozinka
        {
            get
            {
                return lozinka;
            }

            set
            {
                lozinka = value;
            }
        }

        public string Ime
        {
            get
            {
                return ime;
            }

            set
            {
                ime = value;
            }
        }

        public string Prezime
        {
            get
            {
                return prezime;
            }

            set
            {
                prezime = value;
            }
        }

        public Pol Pol
        {
            get
            {
                return pol;
            }

            set
            {
                pol = value;
            }
        }

        public string KontaktTelefon
        {
            get
            {
                return kontaktTelefon;
            }

            set
            {
                kontaktTelefon = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
            }
        }

        public Uloga Uloga
        {
            get
            {
                return uloga;
            }

            set
            {
                uloga = value;
            }
        }

        public List<Voznja> Voznje
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

        public string Jmbg
        {
            get
            {
                return jmbg;
            }

            set
            {
                jmbg = value;
            }
        }
        #endregion

        public override string ToString()
        {
            return Ime + " " + Prezime;
        }
    }
}