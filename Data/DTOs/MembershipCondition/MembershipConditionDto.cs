using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class MembershipConditionDto : BaseDto
    {
        public Guid MembershipConditionId { get; set; }
        public Guid ConditionGroupId { get; set; }
        public bool ForNewMember { get; set; }
        public string MembershipLevel { get; set; }
        public string NextOperator { get; set; }
    }
}
