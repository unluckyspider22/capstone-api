
using Infrastructure.DTOs;
using Infrastructure.DTOs.Voucher;
using Infrastructure.DTOs.VoucherChannel;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IChannelService : IBaseService<Channel, ChannelDto>
    {
        public Task<VoucherForChannelResponse> GetVouchersForChannel(VoucherChannelParam channelParam);
        public Task<List<PromotionInfomation>> GetPromotionsForChannel(VoucherChannelParam channelParam);
    }
}
