using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Services;
using ApplicationCore.Utils;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConditionRulesController : ControllerBase
    {
        private readonly IConditionRuleService _service;

        public ConditionRulesController(IConditionRuleService service)
        {
            _service = service;
        }

        // GET: api/ConditionRules
        [HttpGet]
        public async Task<IActionResult> GetConditionRule([FromQuery] PagingRequestParam param)
        {
            var result = await _service.GetAsync(
                pageIndex: param.PageIndex,
                pageSize: param.PageSize,
                filter: el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE),
                orderBy: el => el.OrderByDescending(b => b.InsDate)
                );

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/ConditionRules/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountConditionRule()
        {
            return Ok(await _service.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)));
        }

        // GET: api/ConditionRules/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConditionRule([FromRoute]Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/ConditionRules/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConditionRule([FromRoute]Guid id, [FromBody] ConditionRuleDto dto)
        {
            if (id != dto.ConditionRuleId)
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

        // POST: api/ConditionRules
        [HttpPost]
        public async Task<IActionResult> PostConditionRule([FromBody] ConditionRuleDto dto)
        {
            dto.ConditionRuleId = Guid.NewGuid();

            var result = await _service.CreateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            //var result = dto;

            return Ok(result);
        }

        // DELETE: api/ConditionRules/5
        [HttpDelete]
        public async Task<IActionResult> DeleteConditionRule([FromQuery]Guid id)
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

        // Put: api/ConditionRules/5
        [HttpPatch]
        public async Task<IActionResult> HideConditionRule([FromQuery]Guid id, [FromQuery]string value)
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