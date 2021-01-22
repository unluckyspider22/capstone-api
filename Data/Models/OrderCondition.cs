using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class OrderCondition
    {
        public Guid OrderConditionId { get; set; }
        public Guid? ConditionRuleId { get; set; }
        public string GroupNo { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Amount { get; set; }
        public string DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }
        public string OperatorQuantity { get; set; }
        public string OperatorAmount { get; set; }

        public virtual ConditionRule ConditionRule { get; set; }
    }
}
