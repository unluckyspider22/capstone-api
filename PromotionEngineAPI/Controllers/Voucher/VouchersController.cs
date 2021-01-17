using ApplicationCore.Models.Voucher;
using ApplicationCore.Services;
using ApplicationCore.Utils;
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
    [Route("api/vouchers")]
    [ApiController]
    public class VouchersController : ControllerBase
    {
        private readonly IVoucherService _service;

        public VouchersController(IVoucherService service)
        {
            _service = service;
        }

        // GET: api/Vouchers
        [HttpGet]
        // api/Vouchers?pageIndex=...&pageSize=...
        public async Task<IActionResult> GetVoucher([FromQuery] PagingRequestParam param)
        {
            var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize, filter: el => el.DelFlg.Equals("0"));
            if (result == null)
            {
                return StatusCode(statusCode: 500, new ErrorResponse().InternalServerError);
            }
            return Ok(result);
        }

        // GET: api/Vouchers/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountVoucher()
        {
            return Ok(await _service.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)));
        }

        // GET: api/Vouchers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoucher([FromRoute] Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return StatusCode(statusCode: 500, new ErrorResponse().InternalServerError);
            }
            return Ok(result);
        }

        // PUT: api/Vouchers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoucher([FromRoute] Guid id, [FromBody] VoucherDto dto)
        {
            if (id != dto.VoucherId)
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }

            dto.UpdDate = DateTime.Now;

            var result = await _service.UpdateAsync(dto);

            if (result == null)
            {

                return StatusCode(statusCode: 500, new ErrorResponse().InternalServerError);
            }

            return Ok(result);

        }

        // POST: api/Vouchers
        [HttpPost]
        public async Task<IActionResult> PostVoucher([FromBody] VoucherDto dto)
        {
            dto.VoucherId = Guid.NewGuid();

            var result = await _service.CreateAsync(dto);

            if (result == null)
            {

                return StatusCode(statusCode: 500, new ErrorResponse().InternalServerError);
            }

            //var result = dto;

            return Ok(result);
        }

        // DELETE: api/Vouchers/5
        [HttpDelete]
        public async Task<IActionResult> DeleteVoucher([FromQuery] Guid id)
        {
            if (id == null)
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            var result = await _service.DeleteAsync(id);
            if (result == false)
            {

                return StatusCode(statusCode: 500, new ErrorResponse().InternalServerError);
            }
            return Ok();
        }

        
    }
}
