
using ApplicationCore.Services;

using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/voucher-channels")]
    [ApiController]
    public class VoucherChannelsController : ControllerBase
    {
        private readonly IVoucherChannelService _service;

        public VoucherChannelsController(IVoucherChannelService service)
        {
            _service = service;
        }
        // GET: api/VoucherChannels
        [HttpGet]
        // api/VoucherChannels?pageIndex=...&pageSize=...
        public async Task<IActionResult> GetVoucherChannel([FromQuery] PagingRequestParam param)
        {
            var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/VoucherChannels/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountVoucherChannel()
        {
            return Ok(await _service.CountAsync());
        }

        // GET: api/VoucherChannels/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoucherChannel([FromRoute] Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/VoucherChannels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoucherChannel([FromRoute] Guid id, [FromBody] VoucherChannelDto dto)
        {
            if (id != dto.VoucherChannelId)
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

        // POST: api/VoucherChannels
        [HttpPost]
        public async Task<IActionResult> PostVoucherChannel([FromBody] VoucherChannelDto dto)
        {
            dto.VoucherChannelId = Guid.NewGuid();

            var result = await _service.CreateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            //var result = dto;

            return Ok(result);
        }

        // DELETE: api/VoucherChannels/5
        [HttpDelete]
        public async Task<IActionResult> DeleteVoucherChannel([FromQuery] Guid id)
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

        
    }
}
