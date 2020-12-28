using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class ProductCondition
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
        public string DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual ConditionRule ConditionRule { get; set; }
    }
}
