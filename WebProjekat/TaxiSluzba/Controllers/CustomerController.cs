using Newtonsoft.Json;
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
    public class CustomerController : ApiController
    {
        [HttpGet, Route("api/Customer/GetCustomerRides")]
        public IHttpActionResult GetCustomerRides()
        {
            IHttpActionResult response;

            Korisnik musterija = (Korisnik)HttpContext.Current.Session["korisnik"];
            List<string> voznje = new List<string>();

            foreach (var voznja in Global.Voznje.Values)
            {
                if (voznja.Musterija.KorisnickoIme == musterija.KorisnickoIme)
                    voznje.Add(voznja.VremePorudzbine.Ticks.ToString());
            }

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(voznje, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            })));

            return response;
        }

        [HttpGet, Route("api/Customer/GetActiveCustomerRides")]
        public IHttpActionResult GetActiveCustomerRides()
        {
            IHttpActionResult response;

            Korisnik musterija = (Korisnik)HttpContext.Current.Session["korisnik"];
            List<Voznja> voznje = new List<Voznja>();

            foreach (var voznja in Global.Voznje.Values)
            {
                if (voznja.Musterija.KorisnickoIme == musterija.KorisnickoIme)
                    voznje.Add(voznja);
            }

            List<string> aktivneVoznje = new List<string>();

            foreach (var voznja in voznje)
            {
                if (voznja.StatusVoznje == Status.KREIRANA_NA_CEKANJU)
                    aktivneVoznje.Add(voznja.VremePorudzbine.Ticks.ToString());
            }

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(aktivneVoznje, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            })));

            return response;
        }

        [HttpPost, Route("api/Customer/CreateRide")]
        public IHttpActionResult CreateRide(HttpRequestMessage value)
        {
            IHttpActionResult response;
            var data = JObject.Parse(value.Content.ReadAsStringAsync().Result);
            double koordinataX;
            Double.TryParse(data["KoordinataX"].ToString(), out koordinataX);
            double koordinataY;
            Double.TryParse(data["KoordinataY"].ToString(), out koordinataY);
            string adresa = data["Adresa"].ToString();
            string tip = data["TipAutomobila"].ToString();

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

            Voznja v = new Voznja();
            Korisnik musterija = (Korisnik)HttpContext.Current.Session["korisnik"];
            v.Musterija = Global.Musterije[musterija.KorisnickoIme];
            Adresa a = new Adresa();
            a.Broj = broj;
            a.Mesto = mesto;
            a.PostanskiBroj = postanski;
            a.Ulica = ulica;
            Lokacija l = new Lokacija();
            l.Adresa = a;
            l.KoordinataX = koordinataX;
            l.KoordinataY = koordinataY;
            v.PocetnaLokacija = l;
            if (tip.Equals("Putničko"))
                v.ZeljeniTipVozila = TipVozila.PUTNICKO;
            else if (tip.Equals("Kombi"))
                v.ZeljeniTipVozila = TipVozila.KOMBI;
            v.StatusVoznje = Status.KREIRANA_NA_CEKANJU;
            v.VremePorudzbine = DateTime.Now;
            //Global.Voznje.Add(v.VremePorudzbine.Ticks.ToString(), v);
            Global.DodajVoznju(v);
            Global.Musterije[musterija.KorisnickoIme].Voznje.Add(v);

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(v, Formatting.Indented, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented,

            })));

            return response;
        }

        [HttpPut, Route("api/Customer/CancelRide")]
        public IHttpActionResult CancelRide(HttpRequestMessage value)
        {
            IHttpActionResult response;
            var data = JObject.Parse(value.Content.ReadAsStringAsync().Result);

            string vremeVoznje = data["VremeVoznje"].ToString();
            Komentar k = new Komentar();
            k.KomentarisanaVoznja = Global.Voznje[vremeVoznje];
            k.Opis = data["Komentar"].ToString();
            k.VlasnikKomentara = (Korisnik)HttpContext.Current.Session["korisnik"];
            k.DatumObjave = DateTime.Now.Date;

            Global.Voznje[vremeVoznje].StatusVoznje = Status.OTKAZANA;
            Global.Voznje[vremeVoznje].Komentari.Add(k);
            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject("Vožnja završena", new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented,

            })));

            return response;
        }
    }
}
