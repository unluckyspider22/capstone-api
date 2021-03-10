using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class ProductCondition
    {
        public ProductCondition()
        {
            ProductConditionMapping = new HashSet<ProductConditionMapping>();
        }

        public Guid ProductConditionId { get; set; }
        public Guid ConditionGroupId { get; set; }
        public int? IndexGroup { get; set; }
        public string ProductConditionType { get; set; }
        public decimal ProductQuantity { get; set; }
        public string QuantityOperator { get; set; }
        public string NextOperator { get; set; }
        public bool DelFlg { get; set; }
        public DateTime InsDate { get; set; }
        public DateTime UpdDate { get; set; }

        public virtual ConditionGroup ConditionGroup { get; set; }
        public virtual ICollection<ProductConditionMapping> ProductConditionMapping { get; set; }
    }
}
