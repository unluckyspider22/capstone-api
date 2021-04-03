using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs
{
    public class ConditionGroupDto :BaseDto
    {
        public Guid ConditionGroupId { get; set; }
        public int GroupNo { get; set; }
        public int NextOperator { get; set; }
        public string Summary { get; set; } = "";
        public ICollection<OrderConditionDto> OrderCondition { get; set; }
        public ICollection<ProductConditionDto> ProductCondition { get; set; }
    }
}
