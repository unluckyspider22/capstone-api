using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class MembershipCondition
    {
        public Guid MembershipConditionId { get; set; }
        public Guid? ConditionRuleId { get; set; }
        public string GroupNo { get; set; }
        public bool ForNewMember { get; set; }
        public string MembershipLevel { get; set; }
        public bool DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual ConditionRule ConditionRule { get; set; }
    }
}
