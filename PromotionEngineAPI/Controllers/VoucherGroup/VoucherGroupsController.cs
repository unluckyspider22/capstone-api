
using ApplicationCore.Services;

using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/voucher-groups")]
    [ApiController]
    public class VoucherGroupsController : ControllerBase
    {
        private readonly IVoucherGroupService _service;

        public VoucherGroupsController(IVoucherGroupService service)
        {
            _service = service;
        }

        // GET: api/VoucherGroups
        [HttpGet]
        // api/VoucherGroups?pageIndex=...&pageSize=...
        public async Task<IActionResult> GetVoucherGroup([FromQuery] SearchPagingRequestParam param, [FromQuery] Guid BrandId, string voucherType)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(param.SearchContent)) param.SearchContent = "";
                if (String.IsNullOrWhiteSpace(voucherType))
                //if (voucherType == null)
                {
                    var resultNofilterVoucherType = await _service.GetAsync(pageIndex: param.PageIndex,
                pageSize: param.PageSize,
                filter: el => el.DelFlg.Equals("0")
                && el.VoucherName.ToLower().Contains(param.SearchContent.ToLower().Trim())
                && el.BrandId.Equals(BrandId));
                    return Ok(resultNofilterVoucherType);
                }
                var result = await _service.GetAsync(pageIndex: param.PageIndex,
                pageSize: param.PageSize,
                filter: el => el.DelFlg.Equals("0")
                && el.BrandId.Equals(BrandId)
                && el.VoucherName.ToLower().Contains(param.SearchContent.ToLower().Trim())
                && el.VoucherType.Equals(voucherType));
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("game")]
        // api/VoucherGroups/game
        public async Task<IActionResult> GetVoucherGroupForGame([FromQuery] PagingRequestParam param, [FromQuery] string BrandCode,
            [FromQuery] string StoreCode)
        {
            try
            {
                if (StoreCode == null || BrandCode == null)
                    return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
                return Ok(await _service.GetVoucherForGame(PageIndex: param.PageIndex, PageSize: param.PageSize, StoreCode: StoreCode, BrandCode: BrandCode)); ;
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

    
        // GET: api/VoucherGroups/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountVoucherGroup()
        {
            try
            {
                return Ok(await _service.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        // GET: api/VoucherGroups/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoucherGroup([FromRoute] Guid id)
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

        // PUT: api/VoucherGroups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoucherGroup([FromRoute] Guid id, [FromBody] VoucherGroupDto dto)
        {
            try
            {
                if (id != dto.VoucherGroupId)
                    return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
                dto.UpdDate = DateTime.Now;
                return Ok(await _service.UpdateAsync(dto));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }

        // POST: api/VoucherGroups
        [HttpPost]
        public async Task<IActionResult> PostVoucherGroup([FromBody] VoucherGroupDto dto)
        {
            try
            {
                dto.VoucherGroupId = Guid.NewGuid();

                if (dto.VoucherType.Equals(AppConstant.ENVIRONMENT_VARIABLE.VOUCHER_TYPE.BULK_CODE))
                {
                    List<VoucherDto> generateVoucher = _service.GenerateBulkCodeVoucher(dto);
                    dto.Voucher = generateVoucher;
                }
                else
                {

                    List<VoucherDto> vouchers = _service.GenerateStandaloneVoucher(dto);
                    dto.Voucher = vouchers;

                }

                var result = await _service.CreateAsync(dto);         

                //var result = dto;

                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        // DELETE: api/VoucherGroups/5
        [HttpDelete]
        public async Task<IActionResult> DeleteVoucherGroup([FromQuery] Guid id)
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
