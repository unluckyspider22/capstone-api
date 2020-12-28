using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class VoucherChannel
    {
        public VoucherChannel()
        {
            Voucher = new HashSet<Voucher>();
        }

        public Guid VoucherChannelId { get; set; }
        public Guid? VoucherGroupId { get; set; }
        public Guid? PromotionId { get; set; }
        public Guid? ChannelId { get; set; }
        public string DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual Channel Channel { get; set; }
        public virtual Promotion Promotion { get; set; }
        public virtual VoucherGroup VoucherGroup { get; set; }
        public virtual ICollection<Voucher> Voucher { get; set; }
    }
}
