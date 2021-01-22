using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class OrderConditionDto : BaseDto
    {
        public Guid OrderConditionId { get; set; }
        public Guid? ConditionRuleId { get; set; }
        public string GroupNo { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; }
        public string OperatorQuantity { get; set; }
        public string OperatorAmount { get; set; }
    }
}
