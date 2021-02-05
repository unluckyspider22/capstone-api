using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class ConditionGroupResponse
    {
        public Guid ConditionGroupId { get; set; }
        public Guid ConditionRuleId { get; set; }
        public decimal GroupNo { get; set; }
        public string NextOperator { get; set; }
        public List<Object> Conditions { get; set; }
    }

    public class ConditionRuleResponse
    {
        public Guid ConditionRuleId { get; set; }
        public Guid? BrandId { get; set; }
        public string RuleName { get; set; }
        public string Description { get; set; }
        public List<ConditionGroupResponse> ConditionGroups { get; set; }
    }
}
