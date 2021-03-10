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
using Infrastructure.DTOs.VoucherChannel;
using Microsoft.AspNetCore.Authorization;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/promotions")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        private readonly IVoucherService _voucherService;
        private readonly IPromotionStoreMappingService _promotionStoreMappingService;


        public PromotionsController(IPromotionService promotionService,
            IPromotionStoreMappingService promotionStoreMappingService,
            IVoucherService voucherService)
        {
            _promotionService = promotionService;
            _promotionStoreMappingService = promotionStoreMappingService;
            _voucherService = voucherService;
        }

        [HttpPost]
        [Route("check-voucher")]
        public async Task<IActionResult> CheckVoucher([FromBody] CustomerOrderInfo orderInfo)
        {
            //Lấy promotion bởi voucher code
            OrderResponseModel responseModel = new OrderResponseModel();
            try
            {
                var promotions = await _voucherService.CheckVoucher(orderInfo);
                if (promotions != null && promotions.Count() > 0)
                {
                    responseModel.CustomerOrderInfo = orderInfo;

                    _promotionService.SetPromotions(promotions);
                    //Check promotion
                    responseModel = await _promotionService.HandlePromotion(responseModel);
                }
                else throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Unmatch_Promotion);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
            return Ok(responseModel);
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
              includeProperties: "PromotionStoreMapping,PromotionTier,PromotionChannelMapping,VoucherGroup"));

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
            try
            {
                return Ok(await _promotionService.CountAsync(el => !el.DelFlg
           && el.BrandId.Equals(BrandId)
           && el.PromotionName.ToLower().Contains(param.SearchContent.ToLower())));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }

        // GET: api/Promotions
        [HttpGet]
        [Route("search")]
        // api/Promotions?SearchContent=...?pageIndex=...&pageSize=...
        public async Task<IActionResult> SearchPromotion([FromQuery] SearchPagingRequestParam param, [FromQuery] Guid BrandId)
        {
            try
            {
                var result = await _promotionService.GetAsync(
                pageIndex: param.PageIndex,
                pageSize: param.PageSize,
                filter: el => !el.DelFlg
                && el.PromotionName.ToLower().Contains(param.SearchContent.ToLower())
                && el.BrandId.Equals(BrandId));
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }


        }

        // GET: api/Promotions/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountSearchResultPromotion([FromQuery] string status, [FromQuery] Guid brandId)
        {
            try
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
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }

        // GET: api/Promotions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotion([FromRoute] Guid id)
        {
            try
            {
                return Ok(await _promotionService.GetFirst(filter: el => el.PromotionId.Equals(id) && !el.DelFlg,
                    includeProperties: "VoucherGroup,PromotionChannelMapping,PromotionStoreMapping"));
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
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _promotionService.DeleteAsync(id);
                if (result == false)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

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
        [HttpPost]
        [Route("create-tier")]
        public async Task<IActionResult> CreatePromotionTier([FromBody] PromotionTierParam promotionTierParam)
        {
            //if (!promotionId.Equals(promotionTierParam.PromotionId))
            //{
            //    return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            //}
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

