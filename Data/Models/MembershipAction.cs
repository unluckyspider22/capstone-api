using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class MembershipAction
    {
        public Guid MembershipActionId { get; set; }
        public string GroupNo { get; set; }
        public Guid? PromotionTierId { get; set; }
        public string ActionType { get; set; }
        public string DiscountType { get; set; }
        public string GiftProductCode { get; set; }
        public string GiftName { get; set; }
        public string GiftVoucherCode { get; set; }
        public decimal? BonusPoint { get; set; }
        public bool DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual PromotionTier PromotionTier { get; set; }
    }
}
