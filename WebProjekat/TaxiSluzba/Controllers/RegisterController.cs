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
                if (k.Uloga == Uloga.VOZAC)
                {
                    Vozac v = new Vozac();
                    v.KorisnickoIme = k.KorisnickoIme;
                    v.Lozinka = k.Lozinka;
                    v.Ime = k.Ime;
                    v.Prezime = k.Prezime;
                    v.Pol = k.Pol;
                    v.Jmbg = k.Jmbg;
                    v.KontaktTelefon = k.KontaktTelefon;
                    v.Email = k.Email;
                    v.Uloga = k.Uloga;
                    v.Lokacija = new Lokacija();
                    v.Lokacija.KoordinataX = 45.24239;
                    v.Lokacija.KoordinataY = 19.8426221;
                    v.Lokacija.Adresa = new Adresa();
                    v.Lokacija.Adresa.Ulica = "Bulevar oslobodjenja";
                    v.Lokacija.Adresa.Broj = 131;
                    v.Lokacija.Adresa.Mesto = "Novi Sad";
                    v.Lokacija.Adresa.PostanskiBroj = 21000;
                    Global.Vozaci.Add(v.KorisnickoIme, v);
                }
                else
                {
                    Musterija m = new Musterija();
                    m.KorisnickoIme = k.KorisnickoIme;
                    m.Lozinka = k.Lozinka;
                    m.Ime = k.Ime;
                    m.Prezime = k.Prezime;
                    m.Pol = k.Pol;
                    m.Jmbg = k.Jmbg;
                    m.KontaktTelefon = k.KontaktTelefon;
                    m.Email = k.Email;
                    m.Uloga = k.Uloga;
                    Global.Musterije.Add(m.KorisnickoIme, m);
                }
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
