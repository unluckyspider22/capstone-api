using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class OrderCondition
    {
        public Guid OrderConditionId { get; set; }
        public Guid ConditionGroupId { get; set; }
        public decimal Quantity { get; set; }
        public string QuantityOperator { get; set; }
        public decimal Amount { get; set; }
        public string AmountOperator { get; set; }
        public bool DelFlg { get; set; }
        public DateTime InsDate { get; set; }
        public DateTime UpdDate { get; set; }

        public virtual ConditionGroup OrderConditionNavigation { get; set; }
    }
}
