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

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(korisnici, Formatting.Indented)));

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

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(korisnici, Formatting.Indented)));

            return response;
        }

        [HttpGet, Route("api/Admin/GetAdminRides")]
        public IHttpActionResult GetAdminRides()
        {
            IHttpActionResult response;

            Korisnik admin = (Korisnik)HttpContext.Current.Session["korisnik"];
            List<Voznja> voznje = new List<Voznja>();

            foreach (var voznja in Global.Voznje)
            {
                if (voznja.Dispecer.KorisnickoIme == admin.KorisnickoIme)
                    voznje.Add(voznja);
            }

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(voznje, Formatting.Indented)));

            return response;
        }

        [HttpGet, Route("api/Admin/GetUnassignedRides")]
        public IHttpActionResult GetUnassignedRides()
        {
            IHttpActionResult response;

            Korisnik admin = (Korisnik)HttpContext.Current.Session["korisnik"];
            List<Voznja> voznje = new List<Voznja>();

            foreach (var voznja in Global.Voznje)
            {
                if (voznja.StatusVoznje == Status.KREIRANA_NA_CEKANJU)
                    voznje.Add(voznja);
            }

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(voznje, Formatting.Indented)));

            return response;
        }

        [HttpGet, Route("api/Admin/GetAllRides")]
        public IHttpActionResult GetAllRides()
        {
            IHttpActionResult response;

            List<Voznja> voznje = new List<Voznja>();

            foreach (var voznja in Global.Voznje)
            {
                voznje.Add(voznja);
            }

            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(voznje, Formatting.Indented)));

            return response;
        }

        [HttpPost, Route("api/Admin/Ban")]
        public IHttpActionResult Ban(HttpRequestMessage value)
        {
            IHttpActionResult response;
            Korisnik k = JsonConvert.DeserializeObject<Korisnik>(JObject.Parse(value.Content.ReadAsStringAsync().Result).ToString());

            if (!Global.Korisnici.Keys.Contains(k.KorisnickoIme))
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, JsonConvert.SerializeObject("Korisnik nije pronađen", Formatting.Indented)));
            else if (Global.Blokirani.Keys.Contains(k.KorisnickoIme))
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, JsonConvert.SerializeObject("Korisnik je već blokiran", Formatting.Indented)));
            else
            {
                Global.Blokirani.Add(k.KorisnickoIme, Global.Korisnici[k.KorisnickoIme]);
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Blokirani[k.KorisnickoIme], Formatting.Indented)));
            }

            return response;
        }

        [HttpDelete, Route("api/Admin/Unban")]
        public IHttpActionResult Unban(HttpRequestMessage value)
        {
            IHttpActionResult response;
            Korisnik k = JsonConvert.DeserializeObject<Korisnik>(JObject.Parse(value.Content.ReadAsStringAsync().Result).ToString());

            if (!Global.Blokirani.Keys.Contains(k.KorisnickoIme))
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, JsonConvert.SerializeObject("Korisnik nije pronađen", Formatting.Indented)));
            else
            {
                Global.Blokirani.Remove(k.KorisnickoIme);
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(k.KorisnickoIme, Formatting.Indented)));
            }

            return response;
        }
        public class test
        {
            public double lat;
            public double lon;
        }
        [HttpPut, Route("api/Admin/GetClosestFreeDrivers")]
        public IHttpActionResult GetClosestFreeDrivers(HttpRequestMessage value)
        {
            IHttpActionResult response;
            Lokacija l = JsonConvert.DeserializeObject<Lokacija>(JObject.Parse(value.Content.ReadAsStringAsync().Result).ToString());
            List<string> najbliziVozaci = new List<string>();

            
                List<Vozac> slobodniVozaci = new List<Vozac>();
                Dictionary<double, string> udaljenostVozaca = new Dictionary<double, string>();

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
                    udaljenostVozaca.Add(udaljenost, vozac.KorisnickoIme);
                }

                udaljenostVozaca = udaljenostVozaca.OrderBy(dic => dic.Key).ToDictionary(dic => dic.Key, dic => dic.Value);

                foreach (var item in udaljenostVozaca.Take(5))
                {
                    najbliziVozaci.Add(item.Value);
                }
            
            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(najbliziVozaci, Formatting.Indented)));

            return response;
        }
    }
}
