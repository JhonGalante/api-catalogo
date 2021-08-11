using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    /*[ApiVersion("1.0", Deprecated = true)]*/
    [ApiVersion("1.0")]
    //[ApiVersion("2.0")]
    //[Route("api/v{v:apiVersion}/teste")]
    [Route("api/teste")]
    [ApiController]
    public class TesteV1Controller : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Content("<html><body><h2>TesteV1Controller - V 1.0 </h2></body></html>");
        }

        //[HttpGet, MapToApiVersion("2.0")]
        //public ActionResult GetVersao2()
        //{
        //    return Content("<html><body><h2>TesteV1Controller - GET V 2.0 </h2></body></html>");
        //}
    }
}
