
using ApplicationCore.Request;
using ApplicationCore.Services;
using Infrastructure.DTOs;
using Infrastructure.DTOs.Voucher;
using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Infrastructure.Helper.AppConstant;

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
        [HttpGet]
        public async Task<IActionResult> GetVoucher(
            [FromQuery] PagingRequestParam param,
            [FromQuery] Guid VoucherGroupId,
            [FromQuery] Guid PromotionId,
            [FromQuery] string SearchCode = "",
            [FromQuery] int VoucherStatus = 1)
        {
            if (SearchCode.Contains("-"))
            {
                SearchCode = SearchCode.Split("-").Last();
            }
            Expression<Func<Voucher, bool>> filter = el => el.VoucherGroupId.Equals(VoucherGroupId)
                                                    && el.VoucherCode.ToUpper().Contains(SearchCode.ToUpper());
            Expression<Func<Voucher, bool>> filter2;
            if (!PromotionId.Equals(Guid.Empty))
            {

                filter2 = el => el.PromotionId.Equals(PromotionId);
                filter = filter.And(filter2);
            }


            if (VoucherStatus > AppConstant.VoucherStatus.ALL)
            {
                switch (VoucherStatus)
                {
                    case AppConstant.VoucherStatus.USED:
                        {
                            filter2 = el => el.IsUsed;
                            filter = filter.And(filter2);
                            break;
                        }
                    case AppConstant.VoucherStatus.UNUSED:
                        {
                            filter2 = el => !el.IsUsed;
                            filter = filter.And(filter2);
                            break;
                        }
                    case AppConstant.VoucherStatus.REDEMPED:
                        {
                            filter2 = el => el.IsRedemped;
                            filter = filter.And(filter2);
                            break;
                        }
                }
            }

            try
            {
                return Ok(await _service.GetAsync(
                pageIndex: param.PageIndex,
                pageSize: param.PageSize,
                filter: filter,
                includeProperties: "Promotion,Channel,Store,Membership",
                orderBy: el => el.OrderBy(obj => obj.Index)
                ));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpGet]
        [Route("check-voucher")]
        public async Task<IActionResult> CheckVoucher(
         [Required][FromQuery] Guid VoucherGroupId,
            [FromQuery] string SearchCode = "")
        {
            if (SearchCode.Contains("-"))
            {
                SearchCode = SearchCode.Split("-").Last();
            }
            Expression<Func<Voucher, bool>> filter = el => el.VoucherGroupId.Equals(VoucherGroupId)
                                                    && el.VoucherCode.ToUpper().Equals(SearchCode.ToUpper());
            try
            {
                return Ok(await _service.GetFirst(
                            filter: filter,
                            includeProperties:
                            "Promotion," +
                            "Channel," +
                            "GameCampaign," +
                            "Membership," +
                            "Store"));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpPost]
        [Route("voucher-on-site/{promotionId}/{tierId}")]
        public async Task<IActionResult> GetVoucherForCustomer([FromBody] VoucherForCustomerModel param, Guid promotionId, Guid tierId)
        {
            try
            {
                await _service.GetVoucherForCusOnSite(param, promotionId, tierId);
                return Ok();
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpGet]
        [Route("urlSession")]
        public IActionResult GenerateSessionForLink([FromQuery] DateTime datetime)
        {
            try
            {
                var encryptext = _service.Encrypt(datetime.ToString());

                return Ok(encryptext);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
        [HttpPost]
        [Route("isExpiredUrl")]
        public IActionResult ValidateSessionLink([FromQuery] string sst)
        {
            try
            {
                var decypted = _service.Decrypt(sst);

                var timeConvert = DateTime.Parse(decypted);

                var result = timeConvert.AddMinutes(2) >= DateTime.Now;

                if (!result)
                {
                    return Forbid();
                }

                return Ok(result);
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
