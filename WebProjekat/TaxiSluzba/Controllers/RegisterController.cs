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
                    //Global.Vozaci.Add(v.KorisnickoIme, v);
                    Global.DodajVozaca(v);
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
                    //Global.Musterije.Add(m.KorisnickoIme, m);
                    Global.DodajMusteriju(m);
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

            Global.IzmeniKorisnika(k);

            if (Global.Dispeceri.Keys.Contains(k.KorisnickoIme))
            {
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Dispeceri[k.KorisnickoIme], Formatting.Indented)));
            }
            else if (Global.Vozaci.Keys.Contains(k.KorisnickoIme))
            {
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Vozaci[k.KorisnickoIme], Formatting.Indented)));
            }
            else if (Global.Musterije.Keys.Contains(k.KorisnickoIme))
            {
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
