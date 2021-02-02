using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class MembershipActionRequestParam
    {
        [StringLength(1)]
        public string ActionType { get; set; }
        [StringLength(1)]
        public string DiscountType { get; set; }
        [Range(0,999999)]
        public decimal? GiftQuantity { get; set; }
        [StringLength(50)]
        public string GiftProductCode { get; set; }
        [StringLength(100)]
        public string GiftName { get; set; }
        [StringLength(100)]
        public string GiftVoucherCode { get; set; }
        [Range(0.00,9999999999.00)]
        public decimal? BonusPoint { get; set; }
    }
}
