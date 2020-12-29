using ApplicationCore.Services.VoucherGroups;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherGroupsController : ControllerBase
    {
        private readonly IVoucherGroupService _service;

        public VoucherGroupsController(IVoucherGroupService service)
        {
            _service = service;
        }

        // GET: api/VoucherGroups
        [HttpGet]
        public List<VoucherGroup> GetVoucherGroup()
        {
            return _service.GetVoucherGroups();
        }

        // GET: api/VoucherGroups/5
        [HttpGet("{id}")]
        public VoucherGroup GetVoucherGroup(Guid id)
        {
            return _service.GetVoucherGroup(id);
        }

        // PUT: api/VoucherGroups/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        public ActionResult PutVoucherGroup(VoucherGroup VoucherGroup)
        {
            if (_service.PutVoucherGroup(VoucherGroup) == 0) return Conflict();
            else return Ok();
        }

        // POST: api/VoucherGroups
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult PostVoucherGroup(VoucherGroup VoucherGroup)
        {
            if (_service.PostVoucherGroup(VoucherGroup) == 0) return Conflict();
            else return Ok();
        }

        // DELETE: api/VoucherGroups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<VoucherGroup>> DeleteVoucherGroup(Guid id)
        {
            if (_service.DeleteVoucherGroup(id) == 0) return NotFound();
            else return Ok();
        }
    }
}
