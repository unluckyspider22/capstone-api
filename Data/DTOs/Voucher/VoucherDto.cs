using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class VoucherDto : BaseDto
    {
        public Guid VoucherId { get; set; }
        public string VoucherCode { get; set; }
        public Guid? VoucherChannelId { get; set; }
        public Guid? VoucherGroupId { get; set; }
        public Guid? MembershipId { get; set; }
        public string IsUsed { get; set; }
        public string IsRedemped { get; set; }
        public DateTime? UsedDate { get; set; }
        public DateTime? RedempedDate { get; set; }
        public string IsActive { get; set; }
    }
}
