using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class VoucherGroupDto : BaseDto
    {
        public Guid VoucherGroupId { get; set; }
        public Guid? PromotionId { get; set; }
        public Guid? BrandId { get; set; }
        public string VoucherName { get; set; }
        public string VoucherType { get; set; }
        public bool IsLimit { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UsedQuantity { get; set; }
        public decimal? RedempedQuantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime? PublicDate { get; set; }

        public string Charset { get; set; }
        public string Prefix { get; set; }

        public string Postfix { get; set; }

        public string CustomCharset { get; set; }

        public string CustomCode { get; set; }

        public int CodeLength { get; set; }

        public virtual ICollection<VoucherDto> Voucher { get; set; }
        public virtual ICollection<VoucherChannelDto> VoucherChannel { get; set; }

    }
}
