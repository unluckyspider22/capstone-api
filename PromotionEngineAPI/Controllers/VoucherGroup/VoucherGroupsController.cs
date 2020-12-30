using ApplicationCore.Models.VoucherGroup;
using ApplicationCore.Services;
using ApplicationCore.Utils;
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
        public ActionResult<VoucherGroupParam> GetVoucherGroup(Guid id)
        {
            VoucherGroupParam result = _service.GetVoucherGroup(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // PUT: api/VoucherGroups/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public ActionResult PutVoucherGroup(Guid id, VoucherGroupParam VoucherGroupParam)
        {
            var result = _service.PutVoucherGroup(id, VoucherGroupParam);
            if (result == GlobalVariables.NOT_FOUND) return NotFound();
            return Ok(VoucherGroupParam);
        }

        //PATCH:  api/VoucherGroups/2?delflg=?
        [HttpPatch("{id}")]
        public ActionResult UpdateDelFlg(Guid id, string delflg)
        {
            var result = _service.UpdateDelFlag(id, delflg);
            if (result == GlobalVariables.NOT_FOUND)
                return NotFound();
            return Ok();
        }

        // POST: api/VoucherGroups
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult PostVoucherGroup(VoucherGroup VoucherGroup)
        {
            var result = _service.PostVoucherGroup(VoucherGroup);
            if (result == GlobalVariables.DUPLICATE) return Conflict();
            return Ok(VoucherGroup);
        }

        // DELETE: api/VoucherGroups/5
        [HttpDelete("{id}")]
        public ActionResult DeleteVoucherGroup(Guid id)
        {
            var result = _service.DeleteVoucherGroup(id);
            if (result == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }
            return Ok();
        }
        // GETCOUNT: api/VoucherGroups/count
        [HttpGet]
        [Route("count")]
        public ActionResult GetStoreCount()
        {
            var result = _service.CountVoucherGroup();
            return Ok(result);
        }
    }
}
