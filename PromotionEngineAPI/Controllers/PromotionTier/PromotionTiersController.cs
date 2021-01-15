using ApplicationCore.Models.PromotionTier;
using ApplicationCore.Services;
using ApplicationCore.Utils;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/promotiontiers")]
    [ApiController]
    public class PromotionTiersController : ControllerBase
    {
        private readonly IPromotionTierService _service;

        public PromotionTiersController(IPromotionTierService service)
        {
            _service = service;
        }

        // GET: api/PromotionTiers
        [HttpGet]
        // api/PromotionTiers?pageIndex=...&pageSize=...
        public async Task<IActionResult> GetPromotionTier([FromQuery] PagingRequestParam param)
        {
            var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize, filter: el => el.DelFlg.Equals("0"));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/PromotionTiers/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountPromotionTier()
        {
            return Ok(await _service.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)));
        }

        // GET: api/PromotionTiers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotionTier([FromRoute] Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/PromotionTiers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPromotionTier([FromRoute] Guid id, [FromBody] PromotionTierDto dto)
        {
            if (id != dto.PromotionTierId)
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

        // POST: api/PromotionTiers
        [HttpPost]
        public async Task<IActionResult> PostPromotionTier([FromBody] PromotionTierDto dto)
        {
            dto.PromotionTierId = Guid.NewGuid();

            var result = await _service.CreateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            //var result = dto;

            return Ok(result);
        }

        // DELETE: api/PromotionTiers/5
        [HttpDelete]
        public async Task<IActionResult> DeletePromotionTier([FromQuery] Guid id)
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

        
    }
}
