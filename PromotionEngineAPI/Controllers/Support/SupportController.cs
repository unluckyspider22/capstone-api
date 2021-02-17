using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers.Support
{
    [Route("api/support")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        [HttpGet]
        [Route("endpoint")]
        public bool SupportAPI()
        {
            return true;
        }
    }
}
