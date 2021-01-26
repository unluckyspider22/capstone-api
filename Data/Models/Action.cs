using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class Action
    {
        public Guid ActionId { get; set; }
        public string ActionType { get; set; }
        public bool IsLimitAmount { get; set; }
        public string ProductCode { get; set; }
        public decimal? DiscountQuantity { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? FixedPrice { get; set; }
        public decimal? MaxAmount { get; set; }
        public bool ApplyLadderPrice { get; set; }
        public bool DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual PromotionTier PromotionTier { get; set; }
    }
}
