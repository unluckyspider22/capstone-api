using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Models.PromotionTier
{
    public class PromotionTierParam
    {
        public Guid PromotionTierId { get; set; }
        public Guid? ConditionRuleId { get; set; }
        public Guid? ActionId { get; set; }
        public Guid? PromotionId { get; set; }
        public Guid? MembershipActionId { get; set; }
        public Guid? VoucherGroupId { get; set; }
    }
}
