using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class PromotionTier
    {
        public Guid PromotionTierId { get; set; }
        public Guid? ConditionRuleId { get; set; }
        public Guid? ActionId { get; set; }
        public Guid? PromotionId { get; set; }
        public Guid? MembershipActionId { get; set; }
        public Guid? VoucherGroupId { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual Action Action { get; set; }
        public virtual ConditionRule ConditionRule { get; set; }
        public virtual MembershipAction MembershipAction { get; set; }
        public virtual Promotion Promotion { get; set; }
    }
}
