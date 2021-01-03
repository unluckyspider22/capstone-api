using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class ConditionRuleDto : BaseDto
    {
        public Guid ConditionRuleId { get; set; }
        public Guid? BrandId { get; set; }
        public string RuleName { get; set; }
        public string Description { get; set; }
    }
}
