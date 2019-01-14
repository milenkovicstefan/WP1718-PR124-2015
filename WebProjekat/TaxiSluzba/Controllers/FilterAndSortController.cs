using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TaxiSluzba.Models;

namespace TaxiSluzba.Controllers
{
    public class FilterAndSortController : ApiController
    {
        [HttpPost, Route("Home/api/FilterAndSort")]
        public IHttpActionResult SetParameters(HttpRequestMessage value)
        {
            IHttpActionResult response;

            //var jo = JObject.Parse(value.Content.ReadAsStringAsync().Result);
            //string parametri = jo.ToString();
            Parametri p = JsonConvert.DeserializeObject<Parametri>(JObject.Parse(value.Content.ReadAsStringAsync().Result).ToString());

            HttpContext.Current.Application["parametri"] = p;
            response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(p, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            })));

            return response;
        }
    }
}
