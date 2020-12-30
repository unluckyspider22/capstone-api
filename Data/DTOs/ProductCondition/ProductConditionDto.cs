using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class ProductConditionDto : BaseDto
    {
        public Guid ProductConditionId { get; set; }
        public Guid? ConditionRuleId { get; set; }
        public string GroupNo { get; set; }
        public string ProductConditionType { get; set; }
        public string ProductType { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductQuantity { get; set; }
        public string ProductTag { get; set; }
    }
}
