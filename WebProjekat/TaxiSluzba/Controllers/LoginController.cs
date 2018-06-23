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
    public class LoginController : ApiController
    {
    // GET: api/Login
    public IEnumerable<string> Get()
        {
            List<string> kor = new List<string>();
            foreach (var k in Global.Korisnici)
                kor.Add(k.Value.ToString());

            return kor;
        }

        // GET: api/Login/5
        public string Get(string korisnicko)
        {
            return "value";
        }

        // POST: api/Login
        public string Post(HttpRequestMessage value)
        {
            var jo = JObject.Parse(value.Content.ReadAsStringAsync().Result);
            var korisnicko = jo["KorisnickoIme"].ToString();
            var lozinka = jo["Lozinka"].ToString();

            if (!Global.Korisnici.Keys.Contains(korisnicko) || !Global.Korisnici[korisnicko].Lozinka.Equals(lozinka))
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Neuspešno logovanje. Proverite korisničko ime i lozinku."),
                    ReasonPhrase = "Error",
                };
                throw new HttpResponseException(response);
            }

            HttpContext.Current.Session["korisnik"] = Global.Korisnici[korisnicko];


            return JsonConvert.SerializeObject(Global.Korisnici[korisnicko], Formatting.Indented);
            
        }

        // PUT: api/Login/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Login/5
        public void Delete(int id)
        {
        }
    }
}
