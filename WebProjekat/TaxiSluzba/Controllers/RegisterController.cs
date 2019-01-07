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
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject("Korisničko ime zauzeto. Pokušajte ponovo.", new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                })));
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
                    Global.RewriteAllTxt();
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
                    Global.RewriteAllTxt();
                }
                Global.Korisnici.Add(k.KorisnickoIme, k);
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Korisnici[k.KorisnickoIme], new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                })));
            }

            return response;
        }

        [HttpPost, Route("api/Register/AddCar")]
        public IHttpActionResult AddCar(HttpRequestMessage value)
        {
            IHttpActionResult response;
            Automobil a = JsonConvert.DeserializeObject<Automobil>(JObject.Parse(value.Content.ReadAsStringAsync().Result).ToString());

            if (Global.Automobili.Keys.Contains(a.TaxiOznaka))
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject("Automobil sa unetom taxi oznakom već postoji.", new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                })));
            else
            {
                a.Vozac = Global.Vozaci[a.Vozac.KorisnickoIme];
                Global.Vozaci[a.Vozac.KorisnickoIme].Automobil = a;
                Global.Automobili.Add(a.TaxiOznaka, a);
                Global.RewriteAllTxt();
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Automobili[a.TaxiOznaka], new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                })));
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
                k.Uloga = Uloga.DISPECER;
                Global.Dispeceri[k.KorisnickoIme].Lozinka = k.Lozinka;
                Global.Dispeceri[k.KorisnickoIme].Ime = k.Ime;
                Global.Dispeceri[k.KorisnickoIme].Prezime = k.Prezime;
                Global.Dispeceri[k.KorisnickoIme].Pol = k.Pol;
                Global.Dispeceri[k.KorisnickoIme].Jmbg = k.Jmbg;
                Global.Dispeceri[k.KorisnickoIme].KontaktTelefon = k.KontaktTelefon;
                Global.Dispeceri[k.KorisnickoIme].Email = k.Email;
                Global.RewriteAllTxt();
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Dispeceri[k.KorisnickoIme], new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                })));
            }
            else if (Global.Vozaci.Keys.Contains(k.KorisnickoIme))
            {
                k.Uloga = Uloga.VOZAC;
                Global.Vozaci[k.KorisnickoIme].Lozinka = k.Lozinka;
                Global.Vozaci[k.KorisnickoIme].Ime = k.Ime;
                Global.Vozaci[k.KorisnickoIme].Prezime = k.Prezime;
                Global.Vozaci[k.KorisnickoIme].Pol = k.Pol;
                Global.Vozaci[k.KorisnickoIme].Jmbg = k.Jmbg;
                Global.Vozaci[k.KorisnickoIme].KontaktTelefon = k.KontaktTelefon;
                Global.Vozaci[k.KorisnickoIme].Email = k.Email;
                Global.Korisnici[k.KorisnickoIme] = (Korisnik)Global.Vozaci[k.KorisnickoIme];
                Global.RewriteAllTxt();
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Vozaci[k.KorisnickoIme], new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                })));
            }
            else if (Global.Musterije.Keys.Contains(k.KorisnickoIme))
            {
                k.Uloga = Uloga.MUSTERIJA;
                Global.Musterije[k.KorisnickoIme].Lozinka = k.Lozinka;
                Global.Musterije[k.KorisnickoIme].Ime = k.Ime;
                Global.Musterije[k.KorisnickoIme].Prezime = k.Prezime;
                Global.Musterije[k.KorisnickoIme].Pol = k.Pol;
                Global.Musterije[k.KorisnickoIme].Jmbg = k.Jmbg;
                Global.Musterije[k.KorisnickoIme].KontaktTelefon = k.KontaktTelefon;
                Global.Musterije[k.KorisnickoIme].Email = k.Email;
                Global.Korisnici[k.KorisnickoIme] = (Korisnik)Global.Musterije[k.KorisnickoIme];
                Global.RewriteAllTxt();
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Korisnici[k.KorisnickoIme], new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                })));
            }
            else
            {
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, JsonConvert.SerializeObject("Korisničko ime zauzeto. Pokušajte ponovo.", new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                })));
            }

            return response;
        }

    }
}
