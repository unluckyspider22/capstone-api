using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class PostAction
    {
        public PostAction()
        {
            PostActionProductMapping = new HashSet<PostActionProductMapping>();
        }

        public Guid PostActionId { get; set; }
        public Guid PromotionTierId { get; set; }
        public string ActionType { get; set; }
        public string DiscountType { get; set; }
        public decimal? GiftQuantity { get; set; }
        public string GiftProductCode { get; set; }
        public string GiftName { get; set; }
        public decimal? BonusPoint { get; set; }
        public bool DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime UpdDate { get; set; }
        public Guid? GiftPromotionId { get; set; }

        public virtual PromotionTier PromotionTier { get; set; }
        public virtual ICollection<PostActionProductMapping> PostActionProductMapping { get; set; }
    }
}
