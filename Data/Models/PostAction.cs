using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class PostAction
    {
        public PostAction()
        {
            PostActionProductMapping = new HashSet<PostActionProductMapping>();
            VoucherGroup = new HashSet<VoucherGroup>();
        }

        public Guid PostActionId { get; set; }
        public Guid? PromotionTierId { get; set; }
        public int ActionType { get; set; }
        public int DiscountType { get; set; }
        public string GiftProductCode { get; set; }
        public decimal? BonusPoint { get; set; }
        public bool DelFlg { get; set; }
        public DateTime InsDate { get; set; }
        public DateTime UpdDate { get; set; }
        public Guid? GiftPromotionId { get; set; }
        public string Name { get; set; }
        public Guid? BrandId { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual PromotionTier PromotionTier { get; set; }
        public virtual ICollection<PostActionProductMapping> PostActionProductMapping { get; set; }
        public virtual ICollection<VoucherGroup> VoucherGroup { get; set; }
    }
}
