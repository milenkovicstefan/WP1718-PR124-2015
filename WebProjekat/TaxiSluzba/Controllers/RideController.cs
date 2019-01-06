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
    public class RideController : ApiController
    {
        // GET: api/Ride
        [HttpGet]
        public IHttpActionResult Get()
        {
            Voznja v = (Voznja)HttpContext.Current.Session["voznja"];

            IHttpActionResult response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(v, new JsonSerializerSettings()
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            })));

            return response;
        }

        // POST: api/Login
        [HttpPost]
        public IHttpActionResult Post(HttpRequestMessage value)
        {
            IHttpActionResult response;

            var jo = JObject.Parse(value.Content.ReadAsStringAsync().Result);
            var voznja = jo["Voznja"].ToString();
            
                HttpContext.Current.Session["voznja"] = Global.Voznje[voznja];
                response = ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Global.Voznje[voznja], new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                })));

            return response;

        }
    }
}