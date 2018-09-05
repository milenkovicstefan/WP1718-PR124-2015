using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaxiSluzba.App_Code;
using TaxiSluzba.Models;

namespace TaxiSluzba.Controllers
{
    public class AccountController : ApiController
    {
        // GET: api/Account/5
        public IHttpActionResult Get(HttpRequestMessage value)
        {
            IHttpActionResult response;

            var jo = JObject.Parse(value.Content.ReadAsStringAsync().Result);
            var korisnicko = jo["KorisnickoIme"].ToString();

            if (!Global.Dispeceri.Keys.Contains(korisnicko) && !Global.Korisnici.Keys.Contains(korisnicko))
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject("Korisnik pod imenom " + korisnicko + " ne postoji.", Formatting.Indented)));
            else if (Global.Dispeceri.Keys.Contains(korisnicko))
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Dispeceri[korisnicko], Formatting.Indented)));
            else
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Korisnici[korisnicko], Formatting.Indented)));

            return response;
        }

        // POST: api/Account
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Account/5
        public IHttpActionResult Put(HttpRequestMessage value)
        {
            IHttpActionResult response;

            var jo = JObject.Parse(value.Content.ReadAsStringAsync().Result);
            var korisnicko = jo["KorisnickoIme"].ToString();

            if (!Global.Dispeceri.Keys.Contains(korisnicko) && !Global.Korisnici.Keys.Contains(korisnicko))
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject("Korisnik pod imenom " + korisnicko + " ne postoji.", Formatting.Indented)));
            else
            {
                Korisnik temp = new Korisnik()
                {
                    Ime = jo["Ime"].ToString(),
                    Prezime = jo["Prezime"].ToString(),
                    KorisnickoIme = jo["KorisnickoIme"].ToString(),
                    Lozinka = jo["Lozinka"].ToString(),
                    Pol = (Pol)Int32.Parse(jo["Pol"].ToString()),
                    Jmbg = jo["Jmbg"].ToString(),
                    KontaktTelefon = jo["KontaktTelefon"].ToString(),
                    Email = jo["Email"].ToString()
                };
                if (Global.Dispeceri.Keys.Contains(korisnicko))
                {
                    Global.Dispeceri[korisnicko] = (Dispecer)temp;
                    response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Dispeceri[korisnicko], Formatting.Indented)));

                }
                else if (Global.Korisnici[korisnicko].Uloga == Uloga.VOZAC)
                {
                    Global.Vozaci[korisnicko] = (Vozac)temp;
                    Global.Korisnici[korisnicko] = temp;
                    response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Korisnici[korisnicko], Formatting.Indented)));
                }
                else
                {
                    Global.Musterije[korisnicko] = (Musterija)temp;
                    Global.Korisnici[korisnicko] = temp;
                    response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Korisnici[korisnicko], Formatting.Indented)));
                }
            }

            return response;
        }

        // DELETE: api/Account/5
        public IHttpActionResult Delete(HttpRequestMessage value)
        {
            IHttpActionResult response;

            var jo = JObject.Parse(value.Content.ReadAsStringAsync().Result);
            var korisnicko = jo["KorisnickoIme"].ToString();

            if (!Global.Dispeceri.Keys.Contains(korisnicko) && !Global.Korisnici.Keys.Contains(korisnicko))
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject("Korisnik pod imenom " + korisnicko + " ne postoji.", Formatting.Indented)));
            else if (Global.Dispeceri.Keys.Contains(korisnicko))
            { 
                Global.Dispeceri.Remove(korisnicko);
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject("Korisnik " + korisnicko + " obrisan.", Formatting.Indented)));
            }
            else
            {
                Global.Korisnici.Remove(korisnicko);
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject("Korisnik " + korisnicko + " obrisan.", Formatting.Indented)));
            }

            return response;
        }
    }
}
