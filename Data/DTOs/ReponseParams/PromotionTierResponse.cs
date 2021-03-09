using Infrastructure.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class PromotionTierResponse
    {
        public ConditionRule conditionRule { get; set; }
        public Action action { get; set; }
        public PostAction membershipAction { get; set; }

        public PromotionTierResponse(ConditionRule conditionRule, Action action, PostAction membershipAction)
        {
            this.conditionRule = conditionRule;
            this.action = action;
            this.membershipAction = membershipAction;
        }
    }
}
