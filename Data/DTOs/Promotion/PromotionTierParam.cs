using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class PromotionTierParam
    {
        [Required]
        public Guid PromotionId { get; set; }
        [Required]
        public ConditionRequestParam ConditionRule { get; set; }
        [Required]
        public List<ConditionGroupDto> ConditionGroups { get; set; }
        [Required]
        public ActionRequestParam Action { get; set; }
        [Required]
        public MembershipActionRequestParam MembershipAction { get; set; }
    }
}
