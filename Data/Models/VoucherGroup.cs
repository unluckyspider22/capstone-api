using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class VoucherGroup
    {
        public VoucherGroup()
        {
            Voucher = new HashSet<Voucher>();
        }

        public Guid VoucherGroupId { get; set; }
        public Guid? PromotionId { get; set; }
        public Guid? BrandId { get; set; }
        public string VoucherName { get; set; }
        public string VoucherType { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UsedQuantity { get; set; }
        public decimal? RedempedQuantity { get; set; }
        public bool IsActive { get; set; }
        public bool DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }
        public string Charset { get; set; }
        public string Postfix { get; set; }
        public string Prefix { get; set; }
        public string CustomCharset { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Promotion Promotion { get; set; }
        public virtual ICollection<Voucher> Voucher { get; set; }
    }
}
