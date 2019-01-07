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
    public class CommentController : ApiController
    {
        // POST: api/Comment
        [HttpPost]
        public IHttpActionResult Post(HttpRequestMessage value)
        {
            IHttpActionResult response;

            var jo = JObject.Parse(value.Content.ReadAsStringAsync().Result);
            var opis = jo["Opis"].ToString();
            var ocena = (Ocena)(Int32.Parse(jo["Ocena"].ToString()));
            var commType = HttpContext.Current.Session["commentType"];
            Voznja voznja = (Voznja)HttpContext.Current.Session["voznja"];
            Korisnik vlasnikKomentara = (Korisnik)HttpContext.Current.Session["korisnik"];
            DateTime datum = DateTime.Now;
            Komentar komentar = new Komentar();
            komentar.DatumObjave = datum;
            komentar.VlasnikKomentara = vlasnikKomentara;
            komentar.KomentarisanaVoznja = Global.Voznje[voznja.VremePorudzbine.Ticks.ToString()];
            komentar.Ocena = ocena;
            komentar.Opis = opis;
            if (commType.Equals("cancel"))
            {
                Global.Voznje[voznja.VremePorudzbine.Ticks.ToString()].Komentari.Add(komentar);
                Global.Voznje[voznja.VremePorudzbine.Ticks.ToString()].StatusVoznje = Status.OTKAZANA;
                Global.RewriteAllTxt();
            }
            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(komentar, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            })));

            return response;

        }
    }
}
