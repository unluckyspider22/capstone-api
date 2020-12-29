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
    public class ChannelsController : ControllerBase
    {
        private readonly IChannelService _service;

        public ChannelsController(IChannelService service)
        {
            _service = service;
        }

        // GET: api/Channels
        [HttpGet]
        public List<Channel> GetChannel()
        {
            return _service.GetChannels();
        }

        // GET: api/Channels/count
        [HttpGet]
        [Route("count")]
        public int CountChannel()
        {
            return _service.CountChannel();
        }

        // GET: api/Channels/5
        [HttpGet("{id}")]
        public Channel GetChannel(Guid id)
        {
            return _service.GetChannels(id);
        }

        // PUT: api/Channels/5
        [HttpPut("{id}")]
        public ActionResult<ChannelParam> PutChannel(Guid id, ChannelParam channelParam)
        {

            if (!id.Equals(channelParam.ChannelId))
            {
                return BadRequest();
            }

            var result = _service.UpdateChannel(id, channelParam);

            if (result == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }

            return Ok(channelParam);
        }

        // POST: api/Channels
        [HttpPost]
        public ActionResult<ChannelParam> PostChannel(ChannelParam ChannelParam)
        {
            ChannelParam.ChannelId = Guid.NewGuid();

            _service.CreateChannel(ChannelParam);

            return Ok(ChannelParam);
        }

        // DELETE: api/Channels/5
        [HttpDelete("{id}")]
        public ActionResult DeleteChannel(Guid id)
        {
            var result = _service.DeleteChannel(id);
            if (result == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}