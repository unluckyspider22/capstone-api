
using ApplicationCore.Request;
using ApplicationCore.Services;

using Infrastructure.DTOs;
using Infrastructure.DTOs.Voucher;
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
        private readonly IVoucherGroupService _voucherGroupService;
        public VouchersController(IVoucherService service, IVoucherGroupService voucherGroupService)
        {
            _service = service;
            _voucherGroupService = voucherGroupService;
        }

        // GET: api/Vouchers
        [HttpGet]
        // api/Vouchers?pageIndex=...&pageSize=...
        public async Task<IActionResult> GetVoucher([FromQuery] PagingRequestParam param, [FromQuery] Guid VoucherGroupId)
        {
            try
            {
                return Ok(await _service.GetAsync(
                pageIndex: param.PageIndex,
                pageSize: param.PageSize,
                filter: el => el.VoucherGroupId.Equals(VoucherGroupId),
                orderBy: el => el.OrderByDescending(obj => obj.InsDate)
                ));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
        [HttpPost]
        [Route("voucher-on-site/{promotionId}")]
        public async Task<IActionResult> GetVoucherForCustomer([FromBody] VoucherForCustomerModel param, Guid promotionId)
        {
            try
            {
                await _service.GetVoucherForCusOnSite(param, promotionId);
                return Ok();
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        // GET: api/Vouchers/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountVoucher([FromQuery] Guid VoucherGroupId)
        {
            try
            {
                return Ok(await _service.CountAsync(el => el.VoucherGroupId.Equals(VoucherGroupId)));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        // GET: api/Vouchers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoucher([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _service.GetByIdAsync(id));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
        // PUT: api/Vouchers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoucher([FromRoute] Guid id, [FromBody] VoucherDto dto)
        {
            try
            {
                if (id != dto.VoucherId)
                    return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
                dto.UpdDate = DateTime.Now;
                return Ok(await _service.UpdateAsync(dto));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }
        // PUT: api/Vouchers/get-voucher-cus
        [HttpPost]
        [Route("get-voucher-cus")]
        public async Task<IActionResult> GetVoucherForCustomer([FromBody] VoucherGroupDto dto)
        {
            try
            {


                return Ok(await _service.GetVoucherForCustomer(dto));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }

        // PUT: api/Vouchers/active
        [HttpPut]
        [Route("update-voucher-applied")]
        public async Task<IActionResult> UpdateVoucherApplied([FromBody] CustomerOrderInfo order)
        {
            try
            {
                return Ok(await _service.UpdateVoucherApplied(order));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }

        // POST: api/Vouchers
        [HttpPost]
        public async Task<IActionResult> PostVoucher([FromBody] VoucherDto dto)
        {
            try
            {
                dto.VoucherId = Guid.NewGuid();
                return Ok(await _service.CreateAsync(dto));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        // DELETE: api/Vouchers/5
        [HttpDelete]
        public async Task<IActionResult> DeleteVoucher([FromQuery] Guid id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }


    }
}
