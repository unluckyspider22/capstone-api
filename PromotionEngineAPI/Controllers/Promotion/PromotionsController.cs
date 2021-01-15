using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Services;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using System.Linq;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/promotions")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        private readonly IPromotionStoreMappingService _promotionStoreMappingService;

        public PromotionsController(IPromotionService service, IPromotionStoreMappingService promotionStoreMappingService)
        {
            _promotionService = service;
            _promotionStoreMappingService = promotionStoreMappingService;
        }

        // GET: api/Promotions
        [HttpGet]
        // api/Promotions?pageIndex=...&pageSize=...
        public async Task<IActionResult> GetPromotion([FromQuery] PagingRequestParam param, [FromQuery] Guid BrandId, [FromQuery] string status)
        {

            if (status.Equals(AppConstant.Status.ALL))
            {
                return Ok(await _promotionService.GetAsync(
                pageIndex: param.PageIndex,
                pageSize: param.PageSize,
                orderBy: el => el.OrderByDescending(b => b.InsDate),
                filter: el => el.DelFlg.Equals("0") && el.BrandId.Equals(BrandId)));
            }
            else return Ok(await _promotionService.GetAsync(
              pageIndex: param.PageIndex,
              pageSize: param.PageSize,
              filter: el => el.DelFlg.Equals("0") && el.BrandId.Equals(BrandId)
              && el.Status.Equals(status)));
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
                filter: el => el.DelFlg.Equals("0")
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
                return Ok(await _promotionService.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)
                && el.BrandId.Equals(brandId)
                && el.Status.Equals(status)));
            }
            return Ok(await _promotionService.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)
            && el.BrandId.Equals(brandId)));
        }

        // GET: api/Promotions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotion([FromRoute]Guid id)
        {
            var result = await _promotionService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/Promotions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPromotion([FromRoute]Guid id, [FromBody] PromotionDto dto)
        {
            if (id != dto.PromotionId)
            {
                return BadRequest();
            }

            dto.UpdDate = DateTime.Now;
            if(dto.PromotionStoreMapping != null)
            {
                await _promotionStoreMappingService.DeletePromotionStoreMapping(dto.PromotionId);
            }
            var result = await _promotionService.UpdateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);

        }

        // POST: api/Promotions
        [HttpPost]
        public async Task<IActionResult> PostPromotion([FromBody] PromotionDto dto)
        {
            dto.PromotionId = Guid.NewGuid();

            var result = await _promotionService.CreateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            //var result = dto;

            return Ok(result);
        }

        // DELETE: api/Promotions/5
        [HttpDelete]
        public async Task<IActionResult> DeletePromotion([FromQuery]Guid id)
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


    }
}
