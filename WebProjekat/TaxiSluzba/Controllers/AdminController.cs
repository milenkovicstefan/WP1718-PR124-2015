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

        [HttpGet, Route("api/Admin/GetClosestFreeDrivers")]
        public IHttpActionResult GetClosestFreeDrivers()
        {
            IHttpActionResult response;

            
            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject("", Formatting.Indented)));

            return response;
        }
    }
}
