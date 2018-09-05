﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public class Voznja
    {
        private DateTime vremePorudzbine;
        private Lokacija pocetnaLokacija;
        private TipVozila zeljeniTipVozila;
        private Musterija musterija;
        private Lokacija odrediste;
        private Dispecer dispecer;
        private double iznos;
        private Komentar komentar;
        private Status statusVoznje;

        #region Properties
        public DateTime VremePorudzbine
        {
            get
            {
                return vremePorudzbine;
            }

            set
            {
                vremePorudzbine = value;
            }
        }

        public Lokacija PocetnaLokacija
        {
            get
            {
                return pocetnaLokacija;
            }

            set
            {
                pocetnaLokacija = value;
            }
        }

        public TipVozila ZeljeniTipVozila
        {
            get
            {
                return zeljeniTipVozila;
            }

            set
            {
                zeljeniTipVozila = value;
            }
        }

        public Musterija Musterija
        {
            get
            {
                return musterija;
            }

            set
            {
                musterija = value;
            }
        }

        public Lokacija Odrediste
        {
            get
            {
                return odrediste;
            }

            set
            {
                odrediste = value;
            }
        }

        public Dispecer Dispecer
        {
            get
            {
                return dispecer;
            }

            set
            {
                dispecer = value;
            }
        }

        public double Iznos
        {
            get
            {
                return iznos;
            }

            set
            {
                iznos = value;
            }
        }

        public Komentar Komentar
        {
            get
            {
                return komentar;
            }

            set
            {
                komentar = value;
            }
        }

        public Status StatusVoznje
        {
            get
            {
                return statusVoznje;
            }

            set
            {
                statusVoznje = value;
            }
        }
        #endregion
    }
}