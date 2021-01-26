using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ApplicationCore.Services;

using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/condition-rules")]
    [ApiController]
    public class ConditionRulesController : ControllerBase
    {
        private readonly IConditionRuleService _service;
        private readonly IProductConditionService _productService;
        private readonly IOrderConditionService _orderService;
        private readonly IMembershipConditionService _membershipService;

        public ConditionRulesController(IConditionRuleService service, IProductConditionService productService, IOrderConditionService orderService, IMembershipConditionService membershipService)
        {
            _service = service;
            _productService = productService;
            _orderService = orderService;
            _membershipService = membershipService;
        }

        // GET: api/ConditionRules
        [HttpGet]
        public async Task<IActionResult> GetConditionRule([FromQuery] PagingRequestParam param, Guid BrandId, bool available)
        {
            Expression<Func<ConditionRule, bool>> filter = el => !el.DelFlg && el.BrandId.Equals(BrandId);
            if (available != null && available)
            {
                filter = el => !el.DelFlg && el.BrandId.Equals(BrandId) && (el.PromotionTier == null);
            }
            var result = await _service.GetAsync(
                pageIndex: param.PageIndex,
                pageSize: param.PageSize,
                filter: el => !el.DelFlg && el.BrandId.Equals(BrandId),
                orderBy: el => el.OrderByDescending(b => b.InsDate),
                includeProperties: "PromotionTier,ProductCondition,OrderCondition,MembershipCondition"
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
            return Ok(await _service.CountAsync(el => !el.DelFlg));
        }

        // GET: api/ConditionRules/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConditionRule([FromRoute] Guid id)
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
        public async Task<IActionResult> PutConditionRule([FromRoute] Guid id, [FromBody] ConditionRuleDto dto)
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
        public async Task<IActionResult> PostConditionRule(
            [FromBody] ConditionParamDto param)
        {
            try
            {
                ConditionParamDto response = new ConditionParamDto();

                // Insert condition rule
                param.conditionRule.ConditionRuleId = Guid.NewGuid();
                response.conditionRule = await _service.CreateAsync(param.conditionRule);

                // Insert list product condition
                foreach (var condition in param.productConditions)
                {
                    condition.ProductConditionId = Guid.NewGuid();
                    condition.ConditionRuleId = param.conditionRule.ConditionRuleId;
                    response.productConditions.Add(await _productService.CreateAsync(condition));
                }
                // Insert list order condition
                foreach (var condition in param.orderConditions)
                {
                    condition.OrderConditionId = Guid.NewGuid();
                    condition.ConditionRuleId = param.conditionRule.ConditionRuleId;
                    response.orderConditions.Add(await _orderService.CreateAsync(condition));
                }
                // Insert list membership condition
                foreach (var condition in param.membershipConditions)
                {
                    condition.MembershipConditionId = Guid.NewGuid();
                    condition.ConditionRuleId = param.conditionRule.ConditionRuleId;
                    response.membershipConditions.Add(await _membershipService.CreateAsync(condition));
                }
                return Ok(response);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }

        // DELETE: api/ConditionRules/5
        [HttpDelete]
        public async Task<IActionResult> DeleteConditionRule([FromQuery] Guid id)
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