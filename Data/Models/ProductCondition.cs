using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class ProductCondition
    {
        public Guid ProductConditionId { get; set; }
        public Guid ConditionGroupId { get; set; }
        public int? IndexGroup { get; set; }
        public string ProductConditionType { get; set; }
        public string ProductType { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal ProductQuantity { get; set; }
        public string QuantityOperator { get; set; }
        public string ParentCode { get; set; }
        public string NextOperator { get; set; }
        public bool DelFlg { get; set; }
        public DateTime InsDate { get; set; }
        public DateTime UpdDate { get; set; }

        public virtual ConditionGroup ConditionGroup { get; set; }
    }
}
