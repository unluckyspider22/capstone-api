
using Microsoft.AspNetCore.Mvc;
using PromotionEngineAPI.Worker;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers.Support
{
    [Route("api/worker")]
    [ApiController]
    public class WokerController : ControllerBase
    {
        private readonly IVoucherWorker _service;
        public WokerController(IVoucherWorker service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{number}")]
        public async Task<IActionResult> TriggerVoucherWoker([FromRoute] int number)
        {
            //_service.StartMonitorLoop(number);
            return Ok();
        }
    }
}
