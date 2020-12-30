using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Models.VoucherChannel
{
    public class VoucherChannelParam
    {
        public Guid VoucherChannelId { get; set; }
        public Guid? VoucherGroupId { get; set; }
        public Guid? PromotionId { get; set; }
        public Guid? ChannelId { get; set; }
    }
}
