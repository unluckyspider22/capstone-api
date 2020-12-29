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
    public class MembershipsController : ControllerBase
    {
        private readonly IMembershipService _service;

        public MembershipsController(IMembershipService service)
        {
            _service = service;
        }

        // GET: api/Memberships
        [HttpGet]
        public List<Membership> GetMembership()
        {
            return _service.FindMembership();
        }

        // GET: api/Memberships/5
        [HttpGet("{id}")]
        public ActionResult<Membership> GetMembership(Guid id)
        {
            var membership = _service.FindMembership(id);

            if (membership == null)
            {
                return NotFound();
            }

            return membership;
        }

        // PUT: api/Memberships/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public ActionResult<Membership> PutMembership(Guid id, MembershipParam param)
        {
            if (id != param.MembershipId)
            {
                return BadRequest();
            }

            try
            {
                int result = _service.UpdateMembership(id, param);
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

        // POST: api/Memberships
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult<Membership> PostMembership(Membership membership)
        {

            int result = _service.AddMembership(membership);
            if (result == GlobalVariables.SUCCESS)
            {
                return Ok();
            }
            else if (result == GlobalVariables.DUPLICATE)
            {
                return Conflict();
            }
            return NoContent();

        }

        // DELETE: api/Memberships/5
        [HttpDelete("{id}")]
        public ActionResult DeleteMembership(Guid id)
        {
            var membership = _service.DeleteMembership(id);

            if (membership > 0)
            {
                return Ok();
            }
            return NotFound();
        }

    }
}
