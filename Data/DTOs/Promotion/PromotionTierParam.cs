using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class PromotionTierParam
    {
        public Guid PromotionId { get; set; }
        public ConditionRequestParam ConditionRule { get; set; }
        public List<ConditionGroupDto> ConditionGroups { get; set; }
        public ActionRequestParam Action { get; set; }
        public MembershipActionRequestParam MembershipAction { get; set; }
    }
}
