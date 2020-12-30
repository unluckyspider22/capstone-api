using ApplicationCore.Models.VoucherChannel;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
   public interface IVoucherChannelService
    {
        public List<VoucherChannel> GetVoucherChannels();

        public VoucherChannel GetVoucherChannel(Guid id);

        public int PostVoucherChannel(VoucherChannel VoucherChannel);

        public int PutVoucherChannel(Guid id, VoucherChannelParam VoucherChannelParam);

        public int DeleteVoucherChannel(Guid id);

        public int CountVoucherChannel();

        public int UpdateDelFlag(Guid id, string delflg);
    }
}
