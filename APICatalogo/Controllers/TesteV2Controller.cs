using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [ApiVersion("2.0")]
    //[Route("api/v{v:apiVersion}/teste")]
    [Route("api/teste2")]
    [ApiController]
    public class TesteV2Controller : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Content("<html><body><h2>TesteV2Controller - V 2.0 </h2></body></html>");
        }
    }
}
