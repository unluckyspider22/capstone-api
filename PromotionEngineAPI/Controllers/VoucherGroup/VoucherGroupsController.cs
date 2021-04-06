
using ApplicationCore.Services;
using ApplicationCore.Worker;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
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

        [HttpGet]
        public async Task<IActionResult> GetVoucherGroup(
            [FromQuery] SearchPagingRequestParam param,
            [FromQuery] Guid BrandId,
            [FromQuery] string voucherType,
            [FromQuery] int ActionType = 0,
            [FromQuery] int PostActionType = 0)
        {
            try
            {


                if (String.IsNullOrWhiteSpace(param.SearchContent)) param.SearchContent = "";
                var myInclude = "";
                Expression<Func<VoucherGroup, bool>> myFilter = el => !el.DelFlg
                                                             && el.BrandId.Equals(BrandId)
                                                             && el.VoucherName.ToLower().Contains(param.SearchContent.ToLower().Trim());
                var compiled = myFilter.Compile();
                if (ActionType > 0)
                {
                    myFilter = el => !el.DelFlg
                    && el.Action.ActionType == ActionType
                                        && el.BrandId.Equals(BrandId)
                                        && el.VoucherName.ToLower().Contains(param.SearchContent.ToLower().Trim());
                }
                else if (PostActionType > 0)
                {
                    myFilter = el => !el.DelFlg
                  && el.PostAction.PostActionType == PostActionType
                                      && el.BrandId.Equals(BrandId)
                                      && el.VoucherName.ToLower().Contains(param.SearchContent.ToLower().Trim());
                }
                var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize, filter: myFilter, includeProperties: myInclude);

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

        /* [HttpGet]
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
         }*/


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
            if (id.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorObj(400, "Required Voucher Group Id"));
            }
            try
            {
                return Ok(await _service.GetDetail(id));
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
        public async Task<IActionResult> DeleteVoucherGroup([FromQuery] Guid voucherGroupId)
        {
            if (voucherGroupId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _service.DeleteVoucherGroup(voucherGroupId);
                _workerService.DeleteVouchers(voucherGroupId: voucherGroupId);
                return Ok(result);
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
        [HttpGet]
        [Route("for-promo/{brandId}")]
        public async Task<IActionResult> GetAvailableVoucherGroup([FromRoute] Guid brandId)
        {
            if (brandId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _service.GetVoucherGroupForPromo(brandId);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpGet]
        [Route("index-info/{voucherGroupId}")]
        public async Task<IActionResult> GetIndexInfo([FromRoute] Guid voucherGroupId)
        {
            if (voucherGroupId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _service.CheckAvailableIndex(voucherGroupId);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }




    }
}
