using ApplicationCore.Request;
using ApplicationCore.Services;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/promotions")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        private readonly IVoucherService _voucherService;
        private readonly IPromotionStoreMappingService _promotionStoreMappingService;
        private readonly IMemberLevelService _memberLevelService;


        public PromotionsController(IPromotionService promotionService,
            IPromotionStoreMappingService promotionStoreMappingService,
            IVoucherService voucherService,
            IMemberLevelService memberLevelService)
        {
            _promotionService = promotionService;
            _promotionStoreMappingService = promotionStoreMappingService;
            _voucherService = voucherService;
            _memberLevelService = memberLevelService;
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
        [HttpPost]
        [Route("check-auto-promotion")]
        public async Task<IActionResult> CheckAutoPromotion([FromBody] CustomerOrderInfo orderInfo, [FromQuery] Guid promotionId)
        {
            //Lấy promotion bởi voucher code
            OrderResponseModel prepareModel = new OrderResponseModel();
            try
            {
                var promotions = await _promotionService.GetAutoPromotions(orderInfo, promotionId);
                if (promotions != null && promotions.Count() > 0)
                {
                    prepareModel.CustomerOrderInfo = orderInfo;
                    _promotionService.SetPromotions(promotions);
                    //Check promotion
                    prepareModel = await _promotionService.HandlePromotion(prepareModel);

                    /*  promotions = _promotionService.GetPromotions();

                      if (promotions.Count > 1)
                      {
                          return Ok(promotions);
                      }*/

                }
                else return StatusCode(statusCode: (int)HttpStatusCode.BadRequest, orderInfo);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, orderInfo);
            }
            return Ok(prepareModel);
        }
        [HttpPost]
        [Route("check-promotion")]
        public async Task<IActionResult> CheckPromotion([FromBody] CustomerOrderInfo orderInfo, [FromQuery] Guid promotionId)
        {
            //Lấy promotion bởi voucher code
            OrderResponseModel responseModel = new OrderResponseModel();
            var vouchers = orderInfo.Vouchers;
            try
            {
                List<Promotion> promotions = null;
                responseModel.CustomerOrderInfo = orderInfo;
                orderInfo.Vouchers = new List<CouponCode>();
                promotions = await _promotionService.GetAutoPromotions(orderInfo, promotionId);
                if (promotions != null && promotions.Count() > 0)
                {

                    _promotionService.SetPromotions(promotions);
                    //Check promotion
                    responseModel = await _promotionService.HandlePromotion(responseModel);

                    promotions = _promotionService.GetPromotions();
                }

                orderInfo.Vouchers = vouchers;
                if (vouchers != null && vouchers.Count() > 0)
                {

                    promotions = await _voucherService.CheckVoucher(orderInfo);
                    if (promotions != null && promotions.Count() > 0)
                    {
                        responseModel.CustomerOrderInfo = orderInfo;

                        _promotionService.SetPromotions(promotions);
                        //Check promotion
                        responseModel = await _promotionService.HandlePromotion(responseModel);
                    }
                }

            }
            catch (ErrorObj e)
            {
                e.Data.Add("order", responseModel);
                return StatusCode(statusCode: e.Code, e);
            }
            return Ok(responseModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetPromotion([FromQuery] PagingRequestParam param, [FromQuery] Guid BrandId, [FromQuery] string status)
        {
            if (status == null) return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            try
            {
                var result = await _promotionService.GetAsync(
                  pageIndex: param.PageIndex,
                  pageSize: param.PageSize,
                  orderBy: el => el.OrderByDescending(b => b.InsDate),
                  filter: HandlePromotionFilter(status, BrandId),
                  includeProperties: "PromotionStoreMapping,PromotionTier.VoucherGroup,PromotionChannelMapping");
                return Ok(result);

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
                    includeProperties: "PromotionChannelMapping,PromotionStoreMapping,MemberLevelMapping,PromotionTier"));
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
                    return StatusCode(statusCode: 400, new ErrorObj(400, "Id should not be empty"));
                }
                if (await _promotionService.GetFirst(filter: o => o.PromotionId.Equals(dto.PromotionId) && !o.DelFlg) == null)
                {
                    return StatusCode(statusCode: 400, new ErrorObj(400, "Promotion Not Found"));
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
              
                var result = await _promotionService.CreatePromotion(dto);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        // DELETE: api/Promotions/5
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeletePromotion([FromRoute] Guid id)
        {
            if (id == null)
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                return Ok(await _promotionService.DeletePromotion(id));
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

        [HttpGet]
        [Route("check-exist-promoCode")]
        public async Task<IActionResult> CheckExistPromoCode([FromQuery]string promoCode, [FromQuery] Guid brandId)
        {
            try
            {
                return Ok(await _promotionService.ExistPromoCode(promoCode: promoCode, brandId:brandId));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
       
        [HttpGet]
        [Route("for-game-campaign/{brandId}")]
        public async Task<IActionResult> GetPromotionGameConfig([FromRoute] Guid brandId)
        {
            try
            {
                return Ok(await _promotionService.GetAsync(filter: o => o.BrandId.Equals(brandId)
                                && o.Status != (int)AppConstant.EnvVar.PromotionStatus.EXPIRED
                                && !o.IsAuto
                                && !o.DelFlg,
                                includeProperties:"PromotionTier.Action"));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
    }

}

