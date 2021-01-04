using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Services;
using Infrastructure.DTOs;
using Infrastructure.Helper;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _service;

        public PromotionsController(IPromotionService service)
        {
            _service = service;
        }

        // GET: api/Promotions
        [HttpGet]
        // api/Promotions?pageIndex=...&pageSize=...
        public async Task<IActionResult> GetPromotion([FromQuery] PagingRequestParam param)
        {
            var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize, filter: el => el.DelFlg.Equals("0"));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        // GET: api/Promotions/count
        [HttpGet]
        [Route("countSearch")]
        public async Task<IActionResult> CountPromotion([FromQuery] SearchPagingRequestParam param)
        {
            return Ok(await _service.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE) && el.PromotionName.ToLower().Contains(param.SearchContent.ToLower())));
        }

        // GET: api/Promotions
        [HttpGet]
        [Route("search")]
        // api/Promotions/SearchContent=...?pageIndex=...&pageSize=...
        public async Task<IActionResult> SearchPromotion([FromQuery] SearchPagingRequestParam param)
        {
            var result = await _service.GetAsync(
                pageIndex: param.PageIndex, 
                pageSize: param.PageSize, 
                filter: el => el.DelFlg.Equals("0") && el.PromotionName.ToLower().Contains(param.SearchContent.ToLower()));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/Promotions/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountSearchResultPromotion()
        {
            return Ok(await _service.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)));
        }

      

        // GET: api/Promotions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotion([FromRoute]Guid id)
        {
            var result = await _service.GetByIdAsync(id);
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

            var result = await _service.UpdateAsync(dto);

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

            var result = await _service.CreateAsync(dto);

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
            var result = await _service.DeleteAsync(id);
            if (result == false)
            {
                return NotFound();
            }
            return Ok();
        }

        // Put: api/Promotions/5
        [HttpPatch]
        public async Task<IActionResult> HidePromotion([FromQuery]Guid id, [FromQuery]string value)
        {
            if (id == null)
            {
                return BadRequest();
            }
            if (!value.Equals(AppConstant.DelFlg.HIDE) && !value.Equals(AppConstant.DelFlg.UNHIDE))
            {
                return BadRequest();
            }
            var result = await _service.HideAsync(id, value);
            if (result == false)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
