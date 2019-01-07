using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using TaxiSluzba.App_Code;
using TaxiSluzba.Models;

namespace TaxiSluzba.Controllers
{
    public class LoginController : ApiController
    {
        // GET: api/Login
        [HttpGet]
        public IHttpActionResult Get()
        {
            Korisnik k = (Korisnik)HttpContext.Current.Session["korisnik"];

            return Ok(JsonConvert.SerializeObject(k, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            }));
        }

        // POST: api/Login
        [HttpPost]
        public IHttpActionResult Post(HttpRequestMessage value)
        {
            IHttpActionResult response;

            var jo = JObject.Parse(value.Content.ReadAsStringAsync().Result);
            var korisnicko = jo["KorisnickoIme"].ToString();
            var lozinka = jo["Lozinka"].ToString();

            if (Global.Korisnici.Keys.Contains(korisnicko) && Global.Korisnici[korisnicko].Lozinka.Equals(lozinka))
            {
                HttpContext.Current.Session["korisnik"] = Global.Korisnici[korisnicko];
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Korisnici[korisnicko], new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                })));
            }
            else if (Global.Dispeceri.Keys.Contains(korisnicko) && Global.Dispeceri[korisnicko].Lozinka.Equals(lozinka))
            {
                HttpContext.Current.Session["korisnik"] = Global.Dispeceri[korisnicko];
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Dispeceri[korisnicko], new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                })));
            }
            else
            {
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject("Neuspešno logovanje. Proverite korisničko ime i lozinku.", new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                })));
            }

            return response;
            
        }
    }
}
