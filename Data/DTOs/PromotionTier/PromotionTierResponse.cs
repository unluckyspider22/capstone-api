using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class PromotionTierResponseParam
    {
        public Guid PromotionTierId { get; set; }
        public Guid? ConditionRuleId { get; set; }
        public Guid? ActionId { get; set; }
        public Guid? PromotionId { get; set; }
        public Guid? MembershipActionId { get; set; }

        public ConditionRuleResponse ConditionRule { get; set; }
        public virtual Models.Action Action { get; set; }
        public virtual MembershipAction MembershipAction { get; set; }
    }
}
