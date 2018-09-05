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
    public class RegisterController : ApiController
    {
        // POST: api/Register
        [HttpPost]
        public IHttpActionResult Post(HttpRequestMessage value)
        {
            IHttpActionResult response;
            Korisnik k = JsonConvert.DeserializeObject<Korisnik>(JObject.Parse(value.Content.ReadAsStringAsync().Result).ToString());

            if (Global.Dispeceri.Keys.Contains(k.KorisnickoIme) || Global.Korisnici.Keys.Contains(k.KorisnickoIme))
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject("Korisničko ime zauzeto. Pokušajte ponovo.", Formatting.Indented)));
            else
            {
                Global.Korisnici.Add(k.KorisnickoIme, k);
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Korisnici[k.KorisnickoIme], Formatting.Indented)));
            }

            return response;
        }

        // PUT: api/Register/5
        [HttpPut]
        public IHttpActionResult Put(HttpRequestMessage value)
        {
            IHttpActionResult response;
            Korisnik k = JsonConvert.DeserializeObject<Korisnik>(JObject.Parse(value.Content.ReadAsStringAsync().Result).ToString());

            if (Global.Dispeceri.Keys.Contains(k.KorisnickoIme))
            {
                Global.Dispeceri[k.KorisnickoIme].Lozinka = k.Lozinka;
                Global.Dispeceri[k.KorisnickoIme].Ime = k.Ime;
                Global.Dispeceri[k.KorisnickoIme].Prezime = k.Prezime;
                Global.Dispeceri[k.KorisnickoIme].Pol = k.Pol;
                Global.Dispeceri[k.KorisnickoIme].Jmbg = k.Jmbg;
                Global.Dispeceri[k.KorisnickoIme].KontaktTelefon = k.KontaktTelefon;
                Global.Dispeceri[k.KorisnickoIme].Email = k.Email;

                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Dispeceri[k.KorisnickoIme], Formatting.Indented)));
            }
            else if (Global.Korisnici.Keys.Contains(k.KorisnickoIme))
            {
                Global.Korisnici[k.KorisnickoIme].Lozinka = k.Lozinka;
                Global.Korisnici[k.KorisnickoIme].Ime = k.Ime;
                Global.Korisnici[k.KorisnickoIme].Prezime = k.Prezime;
                Global.Korisnici[k.KorisnickoIme].Pol = k.Pol;
                Global.Korisnici[k.KorisnickoIme].Jmbg = k.Jmbg;
                Global.Korisnici[k.KorisnickoIme].KontaktTelefon = k.KontaktTelefon;
                Global.Korisnici[k.KorisnickoIme].Email = k.Email;

                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Korisnici[k.KorisnickoIme], Formatting.Indented)));
            }
            else
            {
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, JsonConvert.SerializeObject("Korisničko ime zauzeto. Pokušajte ponovo.", Formatting.Indented)));
            }

            return response;
        }

    }
}