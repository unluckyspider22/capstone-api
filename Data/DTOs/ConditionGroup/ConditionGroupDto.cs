using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class ConditionGroupDto
    {
        public Guid ConditionGroupId { get; set; }
        [Range(0,99999)]
        public decimal GroupNo { get; set; }
        [StringLength(1)]
        public string NextOperator { get; set; }
        public string Summary { get; set; }
        public ICollection<OrderConditionDto> OrderCondition { get; set; }
        public ICollection<ProductConditionDto> ProductCondition { get; set; }
    }
}
