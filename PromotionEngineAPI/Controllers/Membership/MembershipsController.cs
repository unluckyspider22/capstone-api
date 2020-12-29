using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;
using ApplicationCore.Services;

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
        public async Task<ActionResult<Membership>> GetMembership(Guid id)
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
        public ActionResult<Membership> PutMembership(Guid id, Membership membership)
        {


            try
            {
                PutMembership(id, membership);
            }
            catch (DbUpdateConcurrencyException)
            {

                return NotFound();

            }

            return Ok();
        }

        // POST: api/Memberships
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult<Membership> PostMembership(Membership membership)
        {
            try
            {
                var member = _service.AddMembership(membership);
                if (member != 0)
                {
                    return Ok();
                }
            }
            catch (DbUpdateException)
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
            return NoContent();
        }

    }
}
