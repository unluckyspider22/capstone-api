using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class OrderCondition
    {
        public Guid OrderConditionId { get; set; }
        public Guid? ConditionRuleId { get; set; }
        public decimal? MinQuantity { get; set; }
        public decimal? MaxQuantity { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual ConditionRule ConditionRule { get; set; }
    }
}
