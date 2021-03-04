using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ApplicationCore.Services;

using Infrastructure.DTOs;
using Infrastructure.DTOs.VoucherChannel;
using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/channels")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private readonly IChannelService _service;

        public ChannelsController(IChannelService service)
        {
            _service = service;
        }

        // GET: api/Channels
        [HttpGet]
        public async Task<IActionResult> GetChannel([FromQuery] PagingRequestParam param, [FromQuery] Guid BrandId)
        {
            var result = await _service.GetAsync(
                pageIndex: param.PageIndex,
                pageSize: param.PageSize,
                filter: el => !el.DelFlg && el.BrandId.Equals(BrandId),
                orderBy: el => el.OrderByDescending(b => b.InsDate));

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/Channels/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountChannel([FromQuery] Guid BrandId)
        {
            return Ok(await _service.CountAsync(el => !el.DelFlg && el.BrandId.Equals(BrandId)));
        }

        // GET: api/Channels/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChannel([FromRoute]Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/Channels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChannel([FromRoute]Guid id, [FromBody] ChannelDto dto)
        {
            if (id != dto.ChannelId)
            {
                return BadRequest();
            }

            dto.UpdDate = DateTime.Now;

            var result = await _service.UpdateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);

        }

        // POST: api/Channels
        [HttpPost]
        public async Task<IActionResult> PostChannel([FromBody] ChannelDto dto)
        {
            dto.ChannelId = Guid.NewGuid();

            var result = await _service.CreateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            //var result = dto;

            return Ok(result);
        }

        // DELETE: api/Channels/5
        [HttpDelete]
        public async Task<IActionResult> DeleteChannel([FromQuery]Guid id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var result = await _service.DeleteAsync(id);
            if (result == false)
            {
                return NotFound();
            }
            return Ok();
        }
        [HttpGet]
        [Route("{channelCode}/vouchers/{promotionId}")]
        public async Task<IActionResult> GetVoucherForChannel(Guid promotionId, string channelCode, [FromBody] VoucherChannelParam param)
        {
            try
            {
                if (channelCode != param.ChannelCode)
                {
                    return NotFound();
                }
                if (promotionId != param.PromotionId)
                {
                    return NotFound();
                }
                var result = await _service.GetVouchersForChannel(param);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
           
        }

        [HttpGet]
        [Route("{channelCode}/brands/{BrandCode}/promotions")]
        public async Task<IActionResult> GetPromotionForChannel(string channelCode, string BrandCode, [FromBody] VoucherChannelParam param)
        {
            try
            {
                if (!channelCode.Equals(param.ChannelCode))
                {
                    return NotFound();
                }
                if (!BrandCode.Equals(param.BrandCode))
                {
                    return NotFound();
                }
                var result = await _service.GetPromotionsForChannel(param);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }

        [HttpGet]
        [Route("promotion/{promotionId}")]
        public async Task<IActionResult> GetChannelOfPromotion([FromRoute] Guid promotionId, [FromQuery] Guid brandId)
        {
            if (promotionId.Equals(Guid.Empty) || brandId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {

                return Ok(await _service.GetChannelOfPromotion(promotionId: promotionId, brandId: brandId));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpPut]
        [Route("promotion/{promotionId}")]
        public async Task<IActionResult> UpdateChannelOfPromotion([FromRoute] Guid promotionId, [FromBody] UpdateChannelOfPromotion dto)
        {
            if (promotionId.Equals(Guid.Empty) || !promotionId.Equals(dto.PromotionId))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                return Ok(await _service.UpdateChannelOfPromotion(dto: dto));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

    }
}