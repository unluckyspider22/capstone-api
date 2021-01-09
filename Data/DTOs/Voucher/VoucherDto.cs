using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs.Voucher
{
    public class VoucherDto : BaseDto
    {
        public Guid VoucherId { get; set; }
        public string VoucherCode { get; set; }
        public Guid? VoucherChannelId { get; set; }
        public Guid? VoucherGroupId { get; set; }
        public Guid? MembershipId { get; set; }
        public bool? IsUsed { get; set; }
        public bool? IsRedemped { get; set; }
        public DateTime? UsedDate { get; set; }
        public string IsLimited { get; set; }
        public DateTime? RedempedDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
