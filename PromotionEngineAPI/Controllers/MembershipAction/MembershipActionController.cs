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
using Microsoft.EntityFrameworkCore;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipActionsController : ControllerBase
    {
        private readonly IMembershipActionService _service;

        public MembershipActionsController(IMembershipActionService service)
        {
            _service = service;
        }

        // GET: api/MembershipActions
        [HttpGet]
        public ActionResult<List<MembershipAction>> GetMembershipAction()
        {
            return _service.GetMembershipActions();
        }

        // GET: api/MembershipActions/5
        [HttpGet("{id}")]
        public ActionResult<MembershipAction> GetMembershipAction(Guid id)
        {
            var Action = _service.FindMembershipAction(id);

            if (Action == null)
            {
                return NotFound();
            }

            return Ok(Action);
        }

        // PUT: api/MembershipActions/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public ActionResult<MembershipAction> PutMembershipAction(Guid id, MembershipActionParam param)
        {
            if (id != param.MembershipActionId)
            {
                return BadRequest();
            }

            try
            {
                int result = _service.UpdateMembershipAction(id, param);
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

        // POST: api/MembershipActions
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult<MembershipAction> PostMembershipAction(MembershipAction membershipAction)
        {
            int result = _service.AddMembershipAction(membershipAction);
            if (result == GlobalVariables.SUCCESS)
            {
                return Ok(membershipAction);
            }
            else if (result == GlobalVariables.DUPLICATE)
            {
                return Conflict();
            }
            return NoContent();
        }

        // DELETE: api/MembershipActions/5
        [HttpDelete("{id}")]
        public ActionResult DeleteMembershipAction(Guid id)
        {
            var Action = _service.DeleteMembershipAction(id);

            if (Action > 0)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}