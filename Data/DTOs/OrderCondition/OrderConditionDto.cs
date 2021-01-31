using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class OrderConditionDto : BaseDto
    {
        public Guid OrderConditionId { get; set; }
        public Guid ConditionGroupId { get; set; }
        public decimal Quantity { get; set; }
        public string QuantityOperator { get; set; }
        public decimal Amount { get; set; }
        public string AmountOperator { get; set; }
    }
}
