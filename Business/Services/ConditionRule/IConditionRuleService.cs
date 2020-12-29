using ApplicationCore.Models;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IConditionRuleService
    {
        public List<ConditionRule> GetConditionRules();
        public ConditionRule GetConditionRules(Guid id);
        public int CreateConditionRule(ConditionRuleParam conditionRuleParam);
        public int UpdateConditionRule(Guid id, ConditionRuleParam conditionRuleParam);
        public int DeleteConditionRule(Guid id);
        public int CountConditionRule();
    }
}
