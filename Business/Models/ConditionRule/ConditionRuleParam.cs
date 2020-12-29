using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class ConditionRuleParam
    {
        public Guid ConditionRuleId { get; set; }
        public Guid? BrandId { get; set; }
        public string RuleName { get; set; }
        public string Description { get; set; }
    }
}
