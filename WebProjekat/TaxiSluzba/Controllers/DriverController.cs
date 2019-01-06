﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TaxiSluzba.App_Code;
using TaxiSluzba.Models;

namespace TaxiSluzba.Controllers
{
    public class DriverController : ApiController
    {
        [HttpGet, Route("api/Driver/GetDriverRides")]
        public IHttpActionResult GetDriverRides()
        {
            IHttpActionResult response;

            Korisnik vozac = (Korisnik)HttpContext.Current.Session["korisnik"];
            List<Voznja> voznje = new List<Voznja>();

            foreach (var voznja in Global.Voznje.Values)
            {
                if (voznja.Vozac.KorisnickoIme == vozac.KorisnickoIme)
                    voznje.Add(voznja);
            }

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(voznje, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            })));

            return response;
        }

        [HttpGet, Route("api/Driver/GetUnassignedRides")]
        public IHttpActionResult GetUnassignedRides()
        {
            IHttpActionResult response;

            List<Voznja> voznje = new List<Voznja>();

            foreach (var voznja in Global.Voznje.Values)
            {
                if (voznja.StatusVoznje == Status.KREIRANA_NA_CEKANJU)
                    voznje.Add(voznja);
            }

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(voznje, Formatting.Indented)));

            return response;
        }

        [HttpGet, Route("api/Driver/GetActiveDriverRides")]
        public IHttpActionResult GetActiveDriverRides()
        {
            IHttpActionResult response;

            Korisnik vozac = (Korisnik)HttpContext.Current.Session["korisnik"];
            List<Voznja> voznje = new List<Voznja>();

            foreach (var voznja in Global.Voznje.Values)
            {
                if (voznja.Vozac.KorisnickoIme == vozac.KorisnickoIme)
                    voznje.Add(voznja);
            }

            List<Voznja> aktivneVoznje = new List<Voznja>();

            foreach (var voznja in voznje)
            {
                if (voznja.StatusVoznje != Status.USPESNA && voznja.StatusVoznje != Status.NEUSPESNA)
                    aktivneVoznje.Add(voznja);
            }

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(aktivneVoznje, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            })));

            return response;
        }

        [HttpPut, Route("api/Driver/RideFinished")]
        public IHttpActionResult RideFinished(HttpRequestMessage value)
        {
            IHttpActionResult response;
            var data = JObject.Parse(value.Content.ReadAsStringAsync().Result);
            double iznos;
            string vremeVoznje;
            Double.TryParse(data["Iznos"].ToString(), out iznos);
            vremeVoznje = data["VremeVoznje"].ToString();
            Lokacija l = GetLocation(data);

            Global.Voznje[vremeVoznje].Iznos = iznos;
            Global.Voznje[vremeVoznje].Odrediste = l;
            Global.Voznje[vremeVoznje].StatusVoznje = Status.USPESNA;
            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject("Vožnja završena", Formatting.Indented)));

            return response;
        }

        [HttpPut, Route("api/Driver/RideCanceled")]
        public IHttpActionResult RideCanceled(HttpRequestMessage value)
        {
            IHttpActionResult response;
            var data = JObject.Parse(value.Content.ReadAsStringAsync().Result);
            
            string vremeVoznje = data["VremeVoznje"].ToString();
            Komentar k = new Komentar();
            k.KomentarisanaVoznja = Global.Voznje[vremeVoznje];
            k.Opis = data["Komentar"].ToString();
            k.VlasnikKomentara = (Korisnik)HttpContext.Current.Session["korisnik"];
            k.DatumObjave = DateTime.Now.Date;

            Global.Voznje[vremeVoznje].StatusVoznje = Status.NEUSPESNA;
            Global.Voznje[vremeVoznje].Komentari.Add(k);
            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject("Vožnja završena", new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented,

            })));

            return response;
        }

        private Lokacija GetLocation(JObject data)
        {
            double koordinataX;
            Double.TryParse(data["KoordinataX"].ToString(), out koordinataX);
            double koordinataY;
            Double.TryParse(data["KoordinataY"].ToString(), out koordinataY);
            string adresa = data["Adresa"].ToString();
            string[] adresaDelovi = adresa.Split(',');
            string[] ulicaBroj = adresaDelovi[0].Split(' ');
            int broj = Int32.Parse(ulicaBroj.Last());
            string ulica = "";
            for (int i = 0; i < ulicaBroj.Length - 1; i++)
                ulica += ulicaBroj[i] + " ";
            ulica = ulica.Trim();
            string[] mestoPostanski = adresaDelovi[1].Split(' ');
            int postanski = Int32.Parse(mestoPostanski.Last());
            string mesto = "";
            for (int i = 0; i < mestoPostanski.Length - 1; i++)
                mesto += mestoPostanski[i] + " ";
            mesto = mesto.Trim();

            Adresa a = new Adresa();
            a.Broj = broj;
            a.Mesto = mesto;
            a.PostanskiBroj = postanski;
            a.Ulica = ulica;

            Lokacija l = new Lokacija();
            l.Adresa = a;
            l.KoordinataX = koordinataX;
            l.KoordinataY = koordinataY;

            return l;
        }

    }
}
