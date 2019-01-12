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
    public class AdminController : ApiController
    {
        [HttpGet, Route("api/Admin/GetUsers")]
        public IHttpActionResult GetUsers()
        {
            IHttpActionResult response;

            List<string> korisnici = new List<string>();

            foreach (var korisnik in Global.Korisnici.Values)
            {
                if (!Global.Blokirani.Keys.Contains(korisnik.KorisnickoIme))
                    korisnici.Add(korisnik.KorisnickoIme);
            }

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(korisnici, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            })));

            return response;
        }

        [HttpGet, Route("api/Admin/GetBannedUsers")]
        public IHttpActionResult GetBannedUsers()
        {
            IHttpActionResult response;

            List<string> korisnici = new List<string>();

            foreach (var korisnik in Global.Blokirani.Values)
            {
                korisnici.Add(korisnik.KorisnickoIme);
            }

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(korisnici, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            })));

            return response;
        }

        [HttpPost, Route("api/Admin/Search")]
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
            bool vozac1 = !string.IsNullOrWhiteSpace(data["VozacIme"].ToString());
            bool vozac2 = !string.IsNullOrWhiteSpace(data["VozacPrezime"].ToString());
            bool musterija1 = !string.IsNullOrWhiteSpace(data["MusterijaIme"].ToString());
            bool musterija2 = !string.IsNullOrWhiteSpace(data["MusterijaPrezime"].ToString());

            if (vozac1)
                vozacIme = data["VozacIme"].ToString();
            if (vozac2)
                vozacIme = data["VozacPrezime"].ToString();
            if (musterija1)
                vozacIme = data["MusterijaIme"].ToString();
            if (musterija2)
                vozacIme = data["MusterijaPrezime"].ToString();

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
                if (vozac1)
                    if (voznja.Vozac == null)
                        continue;
                    else if (!voznja.Vozac.Ime.Equals(vozacIme))
                        continue;
                if (vozac2)
                    if (voznja.Vozac == null)
                        continue;
                    else if (!voznja.Vozac.Prezime.Equals(vozacPrezime))
                        continue;
                if (musterija1)
                    if (voznja.Musterija == null)
                        continue;
                    else if (!voznja.Musterija.Ime.Equals(musterijaIme))
                        continue;
                if (musterija2)
                    if (voznja.Musterija == null)
                        continue;
                    else if (!voznja.Musterija.Prezime.Equals(musterijaPrezime))
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

        [HttpGet, Route("api/Admin/GetUnassignedRides")]
        public IHttpActionResult GetUnassignedRides()
        {
            IHttpActionResult response;

            Korisnik admin = (Korisnik)HttpContext.Current.Session["korisnik"];
            List<string> voznje = new List<string>();

            foreach (var voznja in Global.Voznje.Values)
            {
                if (voznja.StatusVoznje == Status.KREIRANA_NA_CEKANJU)
                    voznje.Add(voznja.VremePorudzbine.Ticks.ToString());
            }

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(voznje, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            })));

            return response;
        }

        [HttpGet, Route("api/Admin/GetAllRides")]
        public IHttpActionResult GetAllRides()
        {
            IHttpActionResult response;

            List<string> voznje = new List<string>();

            foreach (var voznja in Global.Voznje.Keys)
            {
                voznje.Add(voznja);
            }

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(voznje, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            })));

            return response;
        }

        [HttpGet, Route("api/Admin/GetAdminRides")]
        public IHttpActionResult GetAdminRides()
        {
            IHttpActionResult response;

            Korisnik dispecer = (Korisnik)HttpContext.Current.Session["korisnik"];

            List<string> voznje = new List<string>();

            foreach (var voznja in Global.Voznje.Values)
            {
                if (voznja.Dispecer != null)
                    if (voznja.Dispecer.KorisnickoIme.Equals(dispecer.KorisnickoIme))
                        voznje.Add(voznja.VremePorudzbine.Ticks.ToString());
            }

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(voznje, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            })));

            return response;
        }

        [HttpPost, Route("api/Admin/Ban")]
        public IHttpActionResult Ban(HttpRequestMessage value)
        {
            IHttpActionResult response;
            Korisnik k = JsonConvert.DeserializeObject<Korisnik>(JObject.Parse(value.Content.ReadAsStringAsync().Result).ToString());

            if (!Global.Korisnici.Keys.Contains(k.KorisnickoIme))
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, JsonConvert.SerializeObject("Korisnik nije pronađen", new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                })));
            else if (Global.Blokirani.Keys.Contains(k.KorisnickoIme))
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, JsonConvert.SerializeObject("Korisnik je već blokiran", new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                })));
            else
            {
                Global.Blokirani.Add(k.KorisnickoIme, k);
                Global.RewriteBanovaniTxt();
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Blokirani[k.KorisnickoIme], new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                })));
            }

            return response;
        }

        [HttpPost, Route("api/Admin/CreateRide")]
        public IHttpActionResult CreateRide(HttpRequestMessage value)
        {
            IHttpActionResult response;
            var data = JObject.Parse(value.Content.ReadAsStringAsync().Result);
            double koordinataX;
            Double.TryParse(data["KoordinataX"].ToString(), out koordinataX);
            double koordinataY;
            Double.TryParse(data["KoordinataY"].ToString(), out koordinataY);
            string vozacKorisnicko = data["Vozac"].ToString();
            string tip = data["TipAutomobila"].ToString();

            Voznja v = new Voznja();
            Korisnik dispecer = (Korisnik)HttpContext.Current.Session["korisnik"];
            v.Dispecer = Global.Dispeceri[dispecer.KorisnickoIme];
            Lokacija l = GetLocation(data);
            v.PocetnaLokacija = l;
            if (tip.Equals("Putničko"))
                v.ZeljeniTipVozila = TipVozila.PUTNICKO;
            else if (tip.Equals("Kombi"))
                v.ZeljeniTipVozila = TipVozila.KOMBI;
            else
                v.ZeljeniTipVozila = TipVozila.NEBITNO;
            v.Vozac = Global.Vozaci[vozacKorisnicko];
            v.StatusVoznje = Status.FORMIRANA;
            v.VremePorudzbine = DateTime.Now;
            Global.Voznje.Add(v.VremePorudzbine.Ticks.ToString(), v);
            Global.Vozaci[vozacKorisnicko].Voznje.Add(v);
            Global.Korisnici[vozacKorisnicko].Voznje.Add(v);
            Global.Dispeceri[dispecer.KorisnickoIme].Voznje.Add(v);
            Global.RewriteAllTxt();

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(v, Formatting.Indented, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented,
                
            })));     

            return response;
        }

        [HttpDelete, Route("api/Admin/Unban")]
        public IHttpActionResult Unban(HttpRequestMessage value)
        {
            IHttpActionResult response;
            Korisnik k = JsonConvert.DeserializeObject<Korisnik>(JObject.Parse(value.Content.ReadAsStringAsync().Result).ToString());

            if (!Global.Blokirani.Keys.Contains(k.KorisnickoIme))
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, JsonConvert.SerializeObject("Korisnik nije pronađen", new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                })));
            else
            {
                Global.Blokirani.Remove(k.KorisnickoIme);
                Global.RewriteBanovaniTxt();
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(k.KorisnickoIme, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                })));
            }

            return response;
        }

        [HttpPut, Route("api/Admin/GetClosestFreeDrivers")]
        public IHttpActionResult GetClosestFreeDrivers(HttpRequestMessage value)
        {
            IHttpActionResult response;
            Lokacija l = JsonConvert.DeserializeObject<Lokacija>(JObject.Parse(value.Content.ReadAsStringAsync().Result).ToString());
            List<string> najbliziVozaci = new List<string>();

            
                List<Vozac> slobodniVozaci = new List<Vozac>();
                Dictionary<string, double> udaljenostVozaca = new Dictionary<string, double>();

                foreach (var vozac in Global.Vozaci.Values)
                {
                    if (vozac.Voznje.All(v => v.StatusVoznje == Status.USPESNA || v.StatusVoznje == Status.NEUSPESNA) && !Global.Blokirani.Keys.Contains(vozac.KorisnickoIme))
                    {
                        slobodniVozaci.Add(vozac);
                    }
                }

                foreach (var vozac in slobodniVozaci)
                {
                    double udaljenost = Math.Sqrt(Math.Pow((l.KoordinataX - vozac.Lokacija.KoordinataX), 2) + Math.Pow((l.KoordinataY - vozac.Lokacija.KoordinataY), 2));
                    udaljenostVozaca.Add(vozac.KorisnickoIme, udaljenost);
                }

                udaljenostVozaca = udaljenostVozaca.OrderBy(dic => dic.Value).ToDictionary(dic => dic.Key, dic => dic.Value);

                foreach (var item in udaljenostVozaca.Take(5))
                {
                    najbliziVozaci.Add(item.Key);
                }
            
            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(najbliziVozaci, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
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
