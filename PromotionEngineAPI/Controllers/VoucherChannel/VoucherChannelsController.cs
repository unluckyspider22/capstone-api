using ApplicationCore.Models.VoucherChannel;
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
    public class VoucherChannelsController : ControllerBase
    {
        private readonly IVoucherChannelService _service;

        public VoucherChannelsController(IVoucherChannelService service)
        {
            _service = service;
        }

        // GET: api/VoucherChannels
        [HttpGet]
        public List<VoucherChannel> GetVoucherChannel()
        {
            var result = _service.GetVoucherChannels();
            return result;
        }

        // GET: api/VoucherChannels/5
        [HttpGet("{id}")]
        public ActionResult<VoucherChannelParam> GetVoucherChannel(Guid id)
        {

            VoucherChannel result = _service.GetVoucherChannel(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // PUT: api/VoucherChannels/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public ActionResult<VoucherChannel> PutVoucherChannel(Guid id, VoucherChannelParam VoucherChannelParam)
        {
            var result = _service.PutVoucherChannel(id, VoucherChannelParam);
            if (result == GlobalVariables.NOT_FOUND) return NotFound();
            return Ok(VoucherChannelParam);
        }
        //PATCH:  api/VoucherChannels/2?delflg=?
        [HttpPatch("{id}")]
        public ActionResult UpdateDelFlg(Guid id, string delflg)
        {
            var result = _service.UpdateDelFlag(id, delflg);
            if (result == GlobalVariables.NOT_FOUND)
                return NotFound();
            return Ok();
        }
        // POST: api/VoucherChannels
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult<VoucherChannel> PostVoucherChannel(VoucherChannel VoucherChannel)
        {
            var result = _service.PostVoucherChannel(VoucherChannel);
            if (result == GlobalVariables.DUPLICATE) return Conflict();
            return Ok(VoucherChannel);
        }

        // DELETE: api/VoucherChannels/5
        [HttpDelete("{id}")]
        public ActionResult DeleteVoucherChannel(Guid id)
        {
            var result = _service.DeleteVoucherChannel(id);
            if (result == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }
            return Ok();
        }
        // GETCOUNT: api/VoucherChannels/count
        [HttpGet]
        [Route("count")]
        public ActionResult GetVoucherChannelCount()
        {
            var result = _service.CountVoucherChannel();
            return Ok(result);
        }
    }
}
