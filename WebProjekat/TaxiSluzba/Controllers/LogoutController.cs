using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace TaxiSluzba.Controllers
{
    public class LogoutController : ApiController
    {
        // POST: api/Logout
        [HttpPost]
        public void Post([FromBody]string value)
        {
            HttpContext.Current.Session["korisnik"] = null;
        }
    }
}
