using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class VoucherGroup
    {
        public VoucherGroup()
        {
            VoucherChannel = new HashSet<VoucherChannel>();
        }

        public Guid VoucherGroupId { get; set; }
        public Guid? BrandId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UsedQuantity { get; set; }
        public decimal? RedempedQuantity { get; set; }
        public string Status { get; set; }
        public bool? IsPublic { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? PublicDate { get; set; }
        public string DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<VoucherChannel> VoucherChannel { get; set; }
    }
}
