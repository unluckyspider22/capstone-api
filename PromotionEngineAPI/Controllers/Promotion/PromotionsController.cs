using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Services;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using System.Linq;
using System.Linq.Expressions;
using Infrastructure.Models;
using System.Diagnostics;
using ApplicationCore.Request;
using System.Collections.Generic;
using ApplicationCore.Models;
using System.Net;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/promotions")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        private readonly IVoucherService _voucherService;
        private readonly IPromotionStoreMappingService _promotionStoreMappingService;


        public PromotionsController(IPromotionService service, IPromotionStoreMappingService promotionStoreMappingService, IVoucherService voucherService, IConditionRuleService conditionRuleService)
        {
            _promotionService = service;
            _promotionStoreMappingService = promotionStoreMappingService;
            _voucherService = voucherService;
        }

        [HttpPost]
        [Route("check-voucher")]
        public async Task<IActionResult> CheckVoucher([FromBody] OrderResponseModel responseModel)
        {
            //Lấy promotion bởi voucher code
            var result = responseModel;
            try
            {
                var promotions = await _voucherService.CheckVoucher(responseModel);
                if (promotions != null && promotions.Count() > 0)
                {
                    responseModel.Promotions = promotions;
                    //Check promotion
                    result = await _promotionService.HandlePromotion(responseModel);
                }
                else throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Unmatch_Promotion);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
            return Ok(result);
        }

        // GET: api/Promotions
        [HttpGet]
        // api/Promotions?pageIndex=...&pageSize=...
        public async Task<IActionResult> GetPromotion([FromQuery] PagingRequestParam param, [FromQuery] Guid BrandId, [FromQuery] string status)
        {
            if (status == null) return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            try
            {
                return Ok(await _promotionService.GetAsync(
              pageIndex: param.PageIndex,
              pageSize: param.PageSize,
              orderBy: el => el.OrderByDescending(b => b.InsDate),
              filter: HandlePromotionFilter(status, BrandId),
              includeProperties: "PromotionStoreMapping,PromotionTier,VoucherChannel,VoucherGroup"));

            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }


        }
  
        // GET: api/Promotions/count
        [HttpGet]
        [Route("countSearch")]
        public async Task<IActionResult> CountPromotion([FromQuery] SearchPagingRequestParam param, [FromQuery] Guid BrandId)
        {
            return Ok(await _promotionService.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)
            && el.BrandId.Equals(BrandId)
            && el.PromotionName.ToLower().Contains(param.SearchContent.ToLower())));
        }

        // GET: api/Promotions
        [HttpGet]
        [Route("search")]
        // api/Promotions?SearchContent=...?pageIndex=...&pageSize=...
        public async Task<IActionResult> SearchPromotion([FromQuery] SearchPagingRequestParam param, [FromQuery] Guid BrandId)
        {
            var result = await _promotionService.GetAsync(
                pageIndex: param.PageIndex,
                pageSize: param.PageSize,
                filter: el => !el.DelFlg
                && el.PromotionName.ToLower().Contains(param.SearchContent.ToLower())
                && el.BrandId.Equals(BrandId));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/Promotions/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountSearchResultPromotion([FromQuery] string status, [FromQuery] Guid brandId)
        {
            if (status != null && !status.Equals(AppConstant.Status.ALL))
            {
                return Ok(await _promotionService.CountAsync(el => !el.DelFlg
                && el.BrandId.Equals(brandId)
                && el.Status.Equals(status)));
            }
            return Ok(await _promotionService.CountAsync(el => !el.DelFlg
            && el.BrandId.Equals(brandId)));
        }

        // GET: api/Promotions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotion([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _promotionService.GetFirst(filter: el => el.PromotionId.Equals(id) && !el.DelFlg,
                    includeProperties: "VoucherGroup,VoucherChannel,PromotionStoreMapping"));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }


        }

        // PUT: api/Promotions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPromotion([FromRoute] Guid id, [FromBody] PromotionDto dto)
        {
            try
            {
                if (id != dto.PromotionId || id.Equals(Guid.Empty) || dto.PromotionId.Equals(Guid.Empty))
                {
                    return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
                }
                if (dto.PromotionStoreMapping != null)
                {
                    await _promotionStoreMappingService.DeletePromotionStoreMapping(dto.PromotionId);
                }
                var result = await _promotionService.UpdatePromotion(dto);

                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }


        }

        // POST: api/Promotions
        [HttpPost]
        public async Task<IActionResult> PostPromotion([FromBody] PromotionDto dto)
        {
            try
            {
                dto.PromotionId = Guid.NewGuid();
                dto.InsDate = DateTime.Now;
                dto.UpdDate = DateTime.Now;
                return Ok(await _promotionService.CreateAsync(dto));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        // DELETE: api/Promotions/5
        [HttpDelete]
        public async Task<IActionResult> DeletePromotion([FromQuery] Guid id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var result = await _promotionService.DeleteAsync(id);
            if (result == false)
            {
                return NotFound();
            }
            return Ok();
        }



        Expression<Func<Promotion, bool>> HandlePromotionFilter(String status, Guid BrandId)
        {
            Expression<Func<Promotion, bool>> filterParam;

            if (status.Equals(AppConstant.Status.ALL))
            {

                filterParam = el =>
                !el.DelFlg &&
                el.BrandId.Equals(BrandId);
            }
            else
            {

                filterParam = el =>
                !el.DelFlg &&
                el.BrandId.Equals(BrandId) &&
                el.Status.Equals(status);
            }

            return filterParam;
        }

        [HttpGet]
        [Route("{promotionId}/promotion-tier-detail")]
        public async Task<IActionResult> GetPromotionTierDetail([FromRoute] Guid promotionId)
        {
            //if (!promotionId.Equals(Guid.Empty))
            //{
            //    return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            //}
            try
            {
                return Ok(await _promotionService.GetPromotionTierDetail(promotionId));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }
        [HttpPost]
        [Route("{promotionId}/create-tier")]
        public async Task<IActionResult> CreatePromotionTier([FromRoute] Guid promotionId, [FromBody] PromotionTierParam promotionTierParam)
        {
            if (!promotionId.Equals(promotionTierParam.PromotionId))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                return Ok(await _promotionService.CreatePromotionTier(promotionTierParam: promotionTierParam));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpDelete]
        [Route("{promotionId}/delete-tier")]
        public async Task<IActionResult> DeletePromotionTier([FromRoute] Guid promotionId, [FromBody] DeleteTierRequestParam deleteTierRequestParam)
        {
            if (!promotionId.Equals(deleteTierRequestParam.PromotionId))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                return Ok(await _promotionService.DeletePromotionTier(deleteTierRequestParam: deleteTierRequestParam));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
        [HttpPut]
        [Route("{promotionId}/update-tier")]
        public async Task<IActionResult> UpdatePromotionTier([FromRoute] Guid promotionId, [FromBody] PromotionTierUpdateParam updateParam)
        {
            if (!promotionId.Equals(updateParam.PromotionId))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                return Ok(await _promotionService.UpdatePromotionTier(updateParam: updateParam));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
    }

}

