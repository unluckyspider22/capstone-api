
using ApplicationCore.Services;
using ApplicationCore.Worker;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/voucher-groups")]
    [ApiController]
    public class VoucherGroupsController : ControllerBase
    {
        private readonly IVoucherGroupService _service;
        private readonly IVoucherWorker _workerService;

        public VoucherGroupsController(IVoucherGroupService service, IVoucherWorker workerService)
        {
            _service = service;
            _workerService = workerService;
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
                filter: el => !el.DelFlg
                && el.VoucherName.ToLower().Contains(param.SearchContent.ToLower())
                && el.BrandId.Equals(BrandId));
                    return Ok(resultNofilterVoucherType);
                }
                var result = await _service.GetAsync(pageIndex: param.PageIndex,
                pageSize: param.PageSize,
                filter: el => !el.DelFlg
                && el.BrandId.Equals(BrandId)
                && el.VoucherName.ToLower().Contains(param.SearchContent.ToLower().Trim())
                && el.VoucherType.Equals(voucherType));
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("game")]
        // api/VoucherGroups/game
        public async Task<IActionResult> GetVoucherGroupForGame([FromQuery] PagingRequestParam param, [FromQuery] string brandCode,
            [FromQuery] string storeCode)
        {
            try
            {
                if (storeCode == null || brandCode == null)
                    return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
                return Ok(await _service.GetVoucherGroupForGame(PageIndex: param.PageIndex, PageSize: param.PageSize, StoreCode: storeCode, BrandCode: brandCode)); ;
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e.Message);
            }
        }


        // GET: api/VoucherGroups/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountVoucherGroup()
        {
            try
            {
                return Ok(await _service.CountAsync(el => !el.DelFlg));
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
                await _service.CreateAsync(dto);

                _workerService.InsertVouchers(voucherDto: dto);
                return Ok(dto);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
        [HttpPut]
        [Route("add-more")]
        public async Task<IActionResult> AddMoreVoucher([FromQuery] Guid voucherGroupId, int quantity)
        {
            try
            {
                if (voucherGroupId.Equals(Guid.Empty))
                {
                    return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);

                }

                return Ok(await _service.AddMoreVoucher(voucherGroupId: voucherGroupId, quantity: quantity));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        // DELETE: api/VoucherGroups/5
        [HttpDelete]
        public async Task<IActionResult> DeleteVoucherGroup([FromQuery] Guid voucherGroupId, [FromQuery] Guid promotionId)
        {
            if (voucherGroupId.Equals(Guid.Empty) || promotionId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                //var result = await _service.DeleteVoucherGroup(id);
                _workerService.DeleteVouchers(voucherGroupId: voucherGroupId, promotionId: promotionId);
                //return Ok(result);
                return Ok();
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
        [HttpPut]
        [Route("hide/{id}")]
        public async Task<IActionResult> HideVoucherGroup([FromRoute] Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _service.HideVoucherGroup(id);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpPut]
        [Route("assign")]
        public async Task<IActionResult> AssignVoucherGroup([FromQuery] Guid voucherGroupId, [FromQuery] Guid promotionId)
        {
            if (voucherGroupId.Equals(Guid.Empty) || promotionId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _service.AssignVoucherGroup(promotionId: promotionId, voucherGroupId: voucherGroupId);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpPut]
        [Route("reject")]
        public async Task<IActionResult> RejectVoucherGroup([FromQuery] Guid voucherGroupId, [FromQuery] Guid promotionId)
        {
            if (voucherGroupId.Equals(Guid.Empty) || promotionId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _service.RejectVoucherGroup(voucherGroupId: voucherGroupId, promotionId: promotionId);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
        [HttpGet]
        [Route("available")]
        public async Task<IActionResult> GetAvailableVoucherGroup([FromQuery] Guid brandId, [FromQuery] PagingRequestParam param)
        {
            if (brandId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _service.GetAvailable(PageSize: param.PageSize, PageIndex: param.PageIndex, BrandId: brandId);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }


    }
}
