using ApplicationCore.Models.VoucherChannel;
using ApplicationCore.Services;
using ApplicationCore.Utils;
using Infrastructure.DTOs;
using Infrastructure.DTOs.VoucherChannel;
using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
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
            var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize, filter: el => el.DelFlg.Equals("0"));
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
            return Ok(await _service.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)));
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

        // Put: api/VoucherChannels/5
        [HttpPatch]
        public async Task<IActionResult> HideVoucherChannel([FromQuery] Guid id, [FromQuery] string value)
        {
            if (id == null)
            {
                return BadRequest();
            }
            if (!value.Equals(AppConstant.DelFlg.HIDE) && !value.Equals(AppConstant.DelFlg.UNHIDE))
            {
                return BadRequest();
            }
            var result = await _service.HideAsync(id, value);
            if (result == false)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
