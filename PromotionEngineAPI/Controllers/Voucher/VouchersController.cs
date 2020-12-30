using ApplicationCore.Models.Voucher;
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
    public class VouchersController : ControllerBase
    {
        private readonly IVoucherService _service;

        public VouchersController(IVoucherService service)
        {
            _service = service;
        }

        // GET: api/Vouchers
        [HttpGet]
        public List<Voucher> GetVoucher()
        {
            var result = _service.GetVouchers();
            return result;
        }

        // GET: api/Vouchers/5
        [HttpGet("{id}")]
        public ActionResult<VoucherParam> GetVoucher(Guid id)
        {

            Voucher result = _service.GetVoucher(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // PUT: api/Vouchers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public ActionResult<Voucher> PutVoucher(Guid id, VoucherParam VoucherParam)
        {
            var result = _service.PutVoucher(id, VoucherParam);
            if (result == GlobalVariables.NOT_FOUND) return NotFound();
            return Ok(VoucherParam);
        }
        //PATCH:  api/Vouchers/2?delflg=?
        [HttpPatch("{id}")]
        public ActionResult UpdateDelFlg(Guid id, string delflg)
        {
            var result = _service.UpdateDelFlag(id, delflg);
            if (result == GlobalVariables.NOT_FOUND)
                return NotFound();
            return Ok();
        }
        // POST: api/Vouchers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult<Voucher> PostVoucher(Voucher Voucher)
        {
            var result = _service.PostVoucher(Voucher);
            if (result == GlobalVariables.DUPLICATE) return Conflict();
            return Ok(Voucher);
        }

        // DELETE: api/Vouchers/5
        [HttpDelete("{id}")]
        public ActionResult DeleteVoucher(Guid id)
        {
            var result = _service.DeleteVoucher(id);
            if (result == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }
            return Ok();
        }
        // GETCOUNT: api/Vouchers/count
        [HttpGet]
        [Route("count")]
        public ActionResult GetVoucherCount()
        {
            var result = _service.CountVoucher();
            return Ok(result);
        }
    }
}
