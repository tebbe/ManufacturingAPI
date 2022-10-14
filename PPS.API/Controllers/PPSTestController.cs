using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/PPSTest")]
    public class PPSTestController : ApiController
    {
        [Route("GetTest")]
        [HttpGet]
        public IHttpActionResult GetTest()
        {
            return Ok("Success");
        }
    }
}
