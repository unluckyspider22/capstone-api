using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs.VoucherGroup
{
    public class VoucherGroupDto : BaseDto
    {
        public Guid VoucherGroupId { get; set; }
        public Guid? BrandId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UsedQuantity { get; set; }
        public decimal? RedempedQuantity { get; set; }
        public string Status { get; set; }
        public bool? IsPublic { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? PublicDate { get; set; }
    }
}
