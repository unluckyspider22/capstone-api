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
        public decimal? MinQuantity { get; set; }
        public decimal? MaxQuantity { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public bool? IsContainProduct { get; set; }
        public string ProductType { get; set; }
        public string ProductCode { get; set; }
    }
}
