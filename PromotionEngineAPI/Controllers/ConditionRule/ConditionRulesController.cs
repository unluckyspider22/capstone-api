using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Services;
using ApplicationCore.Utils;
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
        public List<ConditionRule> GetConditionRule()
        {
            return _service.GetConditionRules();
        }

        // GET: api/ConditionRules/count
        [HttpGet]
        [Route("count")]
        public int CountConditionRule()
        {
            return _service.CountConditionRule();
        }

        // GET: api/ConditionRules/5
        [HttpGet("{id}")]
        public ConditionRule GetConditionRule(Guid id)
        {
            return _service.GetConditionRules(id);
        }

        // PUT: api/ConditionRules/5
        [HttpPut("{id}")]
        public ActionResult<ConditionRuleParam> PutConditionRule(Guid id, ConditionRuleParam ConditionRuleParam)
        {

            if (!id.Equals(ConditionRuleParam.ConditionRuleId))
            {
                return BadRequest();
            }

            var result = _service.UpdateConditionRule(id, ConditionRuleParam);

            if (result == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }

            return Ok(ConditionRuleParam);
        }

        // POST: api/ConditionRules
        [HttpPost]
        public ActionResult<ConditionRuleParam> PostConditionRule(ConditionRuleParam ConditionRuleParam)
        {
            ConditionRuleParam.ConditionRuleId = Guid.NewGuid();

            _service.CreateConditionRule(ConditionRuleParam);

            return Ok(ConditionRuleParam);
        }

        // DELETE: api/ConditionRules/5
        [HttpDelete("{id}")]
        public ActionResult DeleteConditionRule(Guid id)
        {
            var result = _service.DeleteConditionRule(id);
            if (result == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}