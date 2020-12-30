using ApplicationCore.Models;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IChannelService
    {
        public List<Channel> GetChannels();
        public Channel GetChannels(Guid id);
        public int CreateChannel(ChannelParam channelParam);
        public int UpdateChannel(Guid id, ChannelParam channelParam);
        public int DeleteChannel(Guid id);
        public int HideChannel(Guid id);
        public int CountChannel();
    }
}
