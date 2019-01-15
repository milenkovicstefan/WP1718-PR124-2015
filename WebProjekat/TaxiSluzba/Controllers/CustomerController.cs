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
                if (voznja.Musterija != null)
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

        [HttpGet, Route("api/Customer/FilterAndSort")]
        public IHttpActionResult FilterAndSort()
        {
            IHttpActionResult response;

            Korisnik musterija = (Korisnik)HttpContext.Current.Session["korisnik"];
            var parametri = (Parametri)HttpContext.Current.Application["parametri"];
            //var data = JObject.Parse(parametri);

            List<Voznja> voznje = new List<Voznja>();

            foreach (var voznja in Global.Voznje)
            {

                if (voznja.Value.Musterija == null)
                    continue;
                if (!voznja.Value.Musterija.KorisnickoIme.Equals(musterija.KorisnickoIme))
                    continue;

                if (parametri.Filter != 7)
                    if (parametri.Filter != (int)voznja.Value.StatusVoznje)
                        continue;

                voznje.Add(voznja.Value);
            }

            List<Voznja> sortiraneVoznje = new List<Voznja>();

            if (parametri.Sort == 1)
                sortiraneVoznje = voznje.OrderBy(o => o.VremePorudzbine).ToList();
            else if (parametri.Sort == 2)
            {
                Dictionary<Voznja, double> ocene = new Dictionary<Voznja, double>();
                foreach (var voznja in voznje)
                    if (voznja.Komentari.Any(o => (int)o.Ocena != 0))
                    {
                        foreach (var komentar in voznja.Komentari)
                            if ((int)komentar.Ocena != 0)
                                ocene.Add(voznja, (int)komentar.Ocena);
                    }
                    else
                        ocene.Add(voznja, 0);

                var sorted = from pair in ocene
                             orderby pair.Value descending
                             select pair;
                foreach (KeyValuePair<Voznja, double> pair in sorted)
                    sortiraneVoznje.Add(pair.Key);
            }
            else
                sortiraneVoznje = voznje;

            List<string> result = new List<string>();
            foreach (var voznja in sortiraneVoznje)
                result.Add(voznja.VremePorudzbine.Ticks.ToString());

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(result, new JsonSerializerSettings()
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
                if (voznja.Musterija != null)
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

        [HttpPost, Route("api/Customer/Search")]
        public IHttpActionResult Search(HttpRequestMessage value)
        {
            IHttpActionResult response;

            var data = JObject.Parse(value.Content.ReadAsStringAsync().Result);
            DateTime datumOd = new DateTime();
            DateTime datumDo = new DateTime();
            int ocenaOd;
            int ocenaDo;
            double cenaOd;
            double cenaDo;
            string vozacIme = string.Empty;
            string vozacPrezime = string.Empty;
            string musterijaIme = string.Empty;
            string musterijaPrezime = string.Empty;

            bool datum1 = DateTime.TryParse(data["DatumOd"].ToString(), out datumOd);
            bool datum2 = DateTime.TryParse(data["DatumDo"].ToString(), out datumDo);
            bool ocena1 = Int32.TryParse(data["OcenaOd"].ToString(), out ocenaOd);
            bool ocena2 = Int32.TryParse(data["OcenaDo"].ToString(), out ocenaDo);
            bool cena1 = double.TryParse(data["CenaOd"].ToString(), out cenaOd);
            bool cena2 = double.TryParse(data["CenaDo"].ToString(), out cenaDo);

            List<string> voznje = new List<string>();

            foreach (var voznja in Global.Voznje.Values)
            {
                if (datum1)
                    if (voznja.VremePorudzbine < datumOd)
                        continue;
                if (datum2)
                    if (voznja.VremePorudzbine > datumDo)
                        continue;
                if (ocena1)
                    if (voznja.Komentari.Any(o => (int)o.Ocena != 0 && (int)o.Ocena < ocenaOd))
                        continue;
                if (ocena2)
                    if (voznja.Komentari.Any(o => (int)o.Ocena != 0 && (int)o.Ocena > ocenaDo))
                        continue;
                if (cena1)
                    if (voznja.Iznos < cenaOd)
                        continue;
                if (cena2)
                    if (voznja.Iznos > cenaDo)
                        continue;

                voznje.Add(voznja.VremePorudzbine.Ticks.ToString());
            }

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(voznje, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            })));

            return response;
        }

        [HttpGet, Route("api/Customer/GetFinishedCustomerRides")]
        public IHttpActionResult GetFinishedCustomerRides()
        {
            IHttpActionResult response;

            Korisnik musterija = (Korisnik)HttpContext.Current.Session["korisnik"];
            List<Voznja> voznje = new List<Voznja>();

            foreach (var voznja in Global.Voznje.Values)
            {
                if (voznja.Musterija != null)
                    if (voznja.Musterija.KorisnickoIme == musterija.KorisnickoIme)
                        voznje.Add(voznja);
            }

            List<string> zavrseneVoznje = new List<string>();

            foreach (var voznja in voznje)
            {
                if (voznja.StatusVoznje == Status.USPESNA || voznja.StatusVoznje == Status.NEUSPESNA)
                    zavrseneVoznje.Add(voznja.VremePorudzbine.Ticks.ToString());
            }

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(zavrseneVoznje, new JsonSerializerSettings()
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
            else
                v.ZeljeniTipVozila = TipVozila.NEBITNO;
            v.StatusVoznje = Status.KREIRANA_NA_CEKANJU;
            v.VremePorudzbine = DateTime.Now;
            Global.Voznje.Add(v.VremePorudzbine.Ticks.ToString(), v);
            Global.Musterije[musterija.KorisnickoIme].Voznje.Add(v);
            Global.RewriteAllTxt();

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

            var jo = JObject.Parse(value.Content.ReadAsStringAsync().Result);
            var voznja = jo["Voznja"].ToString();

            HttpContext.Current.Session["voznja"] = Global.Voznje[voznja];
            HttpContext.Current.Session["commentType"] = "cancelCustomer";

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject("Vožnja završena", new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented,

            })));

            return response;
        }

        [HttpPut, Route("api/Customer/ComentRide")]
        public IHttpActionResult CommentRide(HttpRequestMessage value)
        {
            IHttpActionResult response;

            var jo = JObject.Parse(value.Content.ReadAsStringAsync().Result);
            var voznja = jo["Voznja"].ToString();

            HttpContext.Current.Session["voznja"] = Global.Voznje[voznja];
            HttpContext.Current.Session["commentType"] = "commentCustomer";

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject("Vožnja završena", new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented,

            })));

            return response;
        }
    }
}
