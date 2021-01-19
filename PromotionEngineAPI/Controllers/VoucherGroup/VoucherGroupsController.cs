
using ApplicationCore.Services;

using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.RequestParams;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> GetVoucherGroup([FromQuery] PagingRequestParam param)
        {
            try
            {
                return Ok(await _service.GetAsync(
                filter: el => el.DelFlg.Equals("0"),
                orderBy: el => el.OrderByDescending(obj => obj.InsDate)
                ));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpGet]
        [Route("game")]
        // api/VoucherGroups/game
        public async Task<IActionResult> GetVoucherGroupForGame([FromQuery] string StoreCode,[FromQuery] string BrandCode
           )
        {
            try
            {
                return Ok(await _service.GetAsync(
                filter: el => 
                //điều kiện tiên quyết để lấy voucher cho game (VoucherGroup)
                el.DelFlg.Equals("0") 
                && el.VoucherType.Equals("1") 
                && el.IsActive == true
                && el.PublicDate.Value.CompareTo(DateTime.Now) <= 0
                && el.UsedQuantity < el.Quantity
                && el.RedempedQuantity < el.Quantity
                //điều kiện tùy chọn để lấy voucher cho game (Promotion)
                && !el.Promotion.PromotionType.Equals("2")
                && el.Promotion.Status.Equals("2")
                && el.Promotion.IsActive.Equals("1")
                && el.Promotion.DelFlg.Equals("0")
                && el.Promotion.StartDate.Value.CompareTo(DateTime.Now) <= 0
                && el.Promotion.EndDate.Value.CompareTo(DateTime.Now) >= 0
                //điều kiện tùy chọn để lấy voucher cho game (Brand)
                && el.Promotion.Brand.BrandCode.Equals(BrandCode)
                //điều kiện tùy chọn để lấy voucher cho game (Store)
                && el.Promotion.PromotionStoreMapping.Any(x => x.Store.StoreCode.Equals(StoreCode))
                
                
                
                ));;
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
                return Ok(await _service.CreateAsync(dto));
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
