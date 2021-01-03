using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;
using ApplicationCore.Services;
using ApplicationCore.Utils;
using ApplicationCore.Models.PromotionStoreMapping;
using Infrastructure.Helper;
using Infrastructure.DTOs;
using Infrastructure.DTOs.PromotionStoreMapping;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionStoreMappingsController : ControllerBase
    {
        private readonly IPromotionStoreMappingService _service;

        public PromotionStoreMappingsController(IPromotionStoreMappingService service)
        {
            _service = service;
        }

        // GET: api/PromotionStoreMappings
        [HttpGet]
        // api/PromotionStoreMappings?pageIndex=...&pageSize=...
        public async Task<IActionResult> GetPromotionStoreMapping([FromQuery] PagingRequestParam param)
        {
            var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize, filter: el => el.DelFlg.Equals("0"));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/PromotionStoreMappings/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountPromotionStoreMapping()
        {
            return Ok(await _service.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)));
        }

        // GET: api/PromotionStoreMappings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotionStoreMapping([FromRoute] Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/PromotionStoreMappings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPromotionStoreMapping([FromRoute] Guid id, [FromBody] PromotionStoreMappingDto dto)
        {
            if (id != dto.Id)
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

        // POST: api/PromotionStoreMappings
        [HttpPost]
        public async Task<IActionResult> PostPromotionStoreMapping([FromBody] PromotionStoreMappingDto dto)
        {
            dto.Id = Guid.NewGuid();

            var result = await _service.CreateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            //var result = dto;

            return Ok(result);
        }

        // DELETE: api/PromotionStoreMappings/5
        [HttpDelete]
        public async Task<IActionResult> DeletePromotionStoreMapping([FromQuery] Guid id)
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

        // Put: api/PromotionStoreMappings/5
        [HttpPatch]
        public async Task<IActionResult> HidePromotionStoreMapping([FromQuery] Guid id, [FromQuery] string value)
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
