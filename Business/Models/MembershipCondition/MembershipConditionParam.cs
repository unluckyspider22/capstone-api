using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class MembershipConditionParam
    {
        public Guid MembershipConditionId { get; set; }
        public Guid? ConditionRuleId { get; set; }
        public string GroupNo { get; set; }
        public bool? ForNewMember { get; set; }
        public string MembershipLevel { get; set; }
    }
}
