using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;
using ApplicationCore.Services;
using ApplicationCore.Models;
using ApplicationCore.Utils;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipConditionsController : ControllerBase
    {
        private readonly IMembershipConditionService _service;

        public MembershipConditionsController(IMembershipConditionService service)
        {
            _service = service;
        }

        // GET: api/MembershipConditions
        [HttpGet]
        public ActionResult<List<MembershipCondition>> GetMembershipCondition()
        {
            return _service.GetMembershipConditions();
        }

        // GET: api/MembershipConditions/5
        [HttpGet("{id}")]
        public ActionResult<MembershipCondition> GetMembershipCondition(Guid id)
        {
            var condition = _service.FindMembershipCondition(id);

            if (condition == null)
            {
                return NotFound();
            }

            return Ok(condition);
        }

        // PUT: api/MembershipConditions/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public ActionResult<MembershipCondition> PutMembershipCondition(Guid id, MembershipConditionParam param)
        {
            if (id != param.MembershipConditionId)
            {
                return BadRequest();
            }

            try
            {
                int result = _service.UpdateMembershipCondition(id, param);
                if (result == GlobalVariables.SUCCESS)
                {
                    return Ok(param);
                }
                else if (result == GlobalVariables.NOT_FOUND)
                {
                    return NotFound();
                }
            }
            catch (DbUpdateConcurrencyException)
            {

                return Conflict();

            }

            return NoContent();
        }

        // POST: api/MembershipConditions
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult<MembershipCondition> PostMembershipCondition(MembershipCondition membershipCondition)
        {
            int result = _service.AddMembershipCondition(membershipCondition);
            if (result == GlobalVariables.SUCCESS)
            {
                return Ok(membershipCondition);
            }
            else if (result == GlobalVariables.DUPLICATE)
            {
                return Conflict();
            }
            return NoContent();
        }

        // DELETE: api/MembershipConditions/5
        [HttpDelete("{id}")]
        public ActionResult DeleteMembershipCondition(Guid id)
        {
            var membership = _service.DeleteMembershipCondition(id);

            if (membership > 0)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
