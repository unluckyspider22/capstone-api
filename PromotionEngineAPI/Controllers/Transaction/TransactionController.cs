using ApplicationCore.Request;
using ApplicationCore.Services;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PromotionEngineAPI.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {

        private readonly ITransactionService _service;
        public TransactionController(ITransactionService service)
        {
            _service = service;
        }
        //[Authorize]
        [HttpPost]
        [Route("check-out")]
        public async Task<IActionResult> Checkout([FromBody] OrderResponseModel order,[FromQuery] Guid brandId,[FromQuery] Guid deviceId)
        {
            try
            {
                return Ok(await _service.Checkout(order: order, brandId: brandId, deviceId: deviceId));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
    }
}
