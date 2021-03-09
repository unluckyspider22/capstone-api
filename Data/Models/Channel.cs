using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class Channel
    {
        public Channel()
        {
            PromotionChannelMapping = new HashSet<PromotionChannelMapping>();
            Voucher = new HashSet<Voucher>();
        }

        public Guid ChannelId { get; set; }
        public Guid? BrandId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelCode { get; set; }
        public bool DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }
        public decimal? Group { get; set; }
        public string AccessToken { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<PromotionChannelMapping> PromotionChannelMapping { get; set; }
        public virtual ICollection<Voucher> Voucher { get; set; }
    }
}
