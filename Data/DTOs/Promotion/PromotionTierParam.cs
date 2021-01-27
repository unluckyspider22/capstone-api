using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class PromotionTierParam
    {
        public ConditionParamDto Condition { get; set; }
        public ActionDto Action { get; set; }
        public MembershipActionDto MembershipAction { get; set; }
    }
}
