using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class PromotionStatusDto
    {
        public int Total { get; set; } = 0;
        public int Draft { get; set; } = 0;
        public int Publish { get; set; } = 0;
        public int Unpublish { get; set; } = 0;
        public int Expired { get; set; } = 0;
    }

    public class DistributionStat {
        public List<ChannelVoucherStat> ChannelStat { get; set; }
        public List<StoreVoucherStat> StoreStat { get; set; }
    }
    public class ChannelVoucherStat
    {
        public Guid ChannelId { get; set; }
        public string ChannelName { get; set; }
        public int RedempVoucherCount { get; set; } = 0;
    }

    public class StoreVoucherStat
    {
        public Guid StoreId { get; set; }
        public string StoreName { get; set; }
        public int RedempVoucherCount { get; set; } = 0;
    }
}
