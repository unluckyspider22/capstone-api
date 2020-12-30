using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class MembershipActionParam
    {
        public Guid MembershipActionId { get; set; }
        public Guid? PromotionTierId { get; set; }
        public string GroupNo { get; set; }
        public string ActionType { get; set; }
        public string GiftProductCode { get; set; }
        public string GiftName { get; set; }
        public string GiftVoucherCode { get; set; }
        public decimal? BonusPoint { get; set; }
    }
}
