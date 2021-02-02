using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class MembershipConditionDto
    {
        public bool? ForNewMember { get; set; }
        [StringLength(50)]
        public string MembershipLevel { get; set; }
        [StringLength(1)]
        public string NextOperator { get; set; }
    }
}
