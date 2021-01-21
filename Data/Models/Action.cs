using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class Action
    {
        public Action()
        {
            PromotionTier = new HashSet<PromotionTier>();
        }

        public Guid ActionId { get; set; }
        public Guid? PromotionTierId { get; set; }
        public string GroupNo { get; set; }
        public string ActionType { get; set; }
        public string IsLimitAmount { get; set; }
        public string ProductCode { get; set; }
        public decimal? DiscountQuantity { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? FixedPrice { get; set; }
        public decimal? MaxAmount { get; set; }
        public string ForCurrentProduct { get; set; }
        public string ApplyLadderPrice { get; set; }
        public string DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual ICollection<PromotionTier> PromotionTier { get; set; }
    }
}
