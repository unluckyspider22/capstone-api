using ApplicationCore.Services;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers.Statistic
{
    [Route("api/statistic")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        public StatisticController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet]
        [Route("promotion/status/{brandId}")]
        public async Task<IActionResult> CountPromotionStatus([FromRoute] Guid brandId)
        {

            try
            {
                return Ok(await _promotionService.CountPromotionStatus(brandId: brandId));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }

        [HttpGet]
        [Route("promotion/distribution/{promotionId}")]
        public async Task<IActionResult> DistributionCount([FromRoute] Guid promotionId)
        {

            try
            {
                return Ok(await _promotionService.DistributionStatistic(promotionId: promotionId));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }

    }
}
