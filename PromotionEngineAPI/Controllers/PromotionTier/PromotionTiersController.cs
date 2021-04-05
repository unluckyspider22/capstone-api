
using ApplicationCore.Services;

using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/promotion-tiers")]
    [ApiController]
    public class PromotionTiersController : ControllerBase
    {
        private readonly IPromotionTierService _service;

        public PromotionTiersController(IPromotionTierService service)
        {
            _service = service;
        }

        // GET: api/PromotionTiers
        //[HttpGet]
        //// api/PromotionTiers?pageIndex=...&pageSize=...
        //public async Task<IActionResult> GetPromotionTier([FromQuery] PagingRequestParam param)
        //{
        //    var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize);
        //    if (result == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(result);
        //}

        // GET: api/PromotionTiers/count
        //[HttpGet]
        //[Route("count")]
        //public async Task<IActionResult> CountPromotionTier()
        //{
        //    return Ok(await _service.CountAsync());
        //}

        // GET: api/PromotionTiers/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetPromotionTier([FromRoute] Guid id)
        //{
        //    var result = await _service.GetByIdAsync(id);
        //    if (result == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(result);
        //}

        // PUT: api/PromotionTiers/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPromotionTier([FromRoute] Guid id, [FromBody] PromotionTierDto dto)
        //{
        //    if (id != dto.PromotionTierId)
        //    {
        //        return BadRequest();
        //    }

        //    dto.UpdDate = DateTime.Now;

        //    var result = await _service.UpdateAsync(dto);

        //    if (result == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(result);

        //}

        // POST: api/PromotionTiers
        [HttpPost]
        public async Task<IActionResult> PostPromotionTier([FromBody] PromotionTierDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        // DELETE: api/PromotionTiers/5
        //[HttpDelete]
        //public async Task<IActionResult> DeletePromotionTier([FromQuery] Guid id)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    var result = await _service.DeleteAsync(id);
        //    if (result == false)
        //    {
        //        return NotFound();
        //    }
        //    return Ok();
        //}

        [HttpGet]
        [Route("available")]
        public async Task<IActionResult> GetAvailable([FromQuery] PagingRequestParam param, [FromQuery] Guid brandId, [FromQuery] string actionType, [FromQuery] string discountType)
        {
            try
            {
                return Ok(await _service.GetAvailable(pageSize: param.PageSize, pageIndex: param.PageIndex, brandId: brandId, actionType: actionType, discountType: discountType));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpPut]
        [Route("assign")]
        public async Task<IActionResult> AssignTierToPromo([FromQuery] Guid promotionId, [FromQuery] Guid promotionTierId)
        {
            if (promotionId.Equals(Guid.Empty) || promotionTierId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                return Ok(await _service.AssignTierToPromo(promotionId: promotionId, promotionTierId: promotionTierId));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }


    }
}
