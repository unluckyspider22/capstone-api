using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Utils;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services
{
    public class ChannelService : IChannelService
    {
        private readonly PromotionEngineContext _context;
        public ChannelService(PromotionEngineContext context)
        {
            _context = context;
        }
        public int CountChannel()
        {
            return _context.Channel.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).Count();
        }

        public int CreateChannel(ChannelParam channelParam)
        {
            channelParam.ChannelId = Guid.NewGuid();
            //var channel = _mapper.Map<Channel>(channelParam);
            var channel = new Channel
            {
                ChannelId = channelParam.ChannelId,
                BrandId = channelParam.BrandId,
                ChannelName = channelParam.ChannelName,
                ChannelCode = channelParam.ChannelCode
            };

            _context.Channel.Add(channel);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return GlobalVariables.SUCCESS;
        }

        public int DeleteChannel(Guid id)
        {
            var channel = _context.Channel.Find(id);

            if (channel == null)
            {
                return GlobalVariables.NOT_FOUND;
            }

            channel.DelFlg = GlobalVariables.DELETED;
            try
            {
                _context.Entry(channel).Property("DelFlg").IsModified = true;
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }


            return GlobalVariables.SUCCESS;
        }

        public List<Channel> GetChannels()
        {
            return _context.Channel.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).ToList();
        }

        public Channel GetChannels(Guid id)
        {
            return _context.Channel
               .Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED))
               .Where(c => c.ChannelId.Equals(id))
               .First();
        }

        public int UpdateChannel(Guid id, ChannelParam channelParam)
        {
            var channel = _context.Channel.Find(id);

            if (channel == null)
            {
                return GlobalVariables.NOT_FOUND;
            }

            channel.ChannelId = channelParam.ChannelId;
            channel.BrandId = channelParam.BrandId;
            channel.ChannelName = channelParam.ChannelName;
            channel.ChannelCode = channelParam.ChannelCode;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return GlobalVariables.SUCCESS;
        }
    }
}
