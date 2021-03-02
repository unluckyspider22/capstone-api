using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class PromotionTierDto : BaseDto
    {
        public Guid PromotionTierId { get; set; }
        public Guid? ConditionRuleId { get; set; }
        public Guid? ActionId { get; set; }
        public Guid? PromotionId { get; set; }
        public Guid? MembershipActionId { get; set; }
        public string Summary { get; set; }
    }
}
