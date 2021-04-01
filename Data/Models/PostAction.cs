using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class PostAction
    {
        public PostAction()
        {
            PostActionProductMapping = new HashSet<PostActionProductMapping>();
            PromotionTier = new HashSet<PromotionTier>();
            VoucherGroup = new HashSet<VoucherGroup>();
        }

        public Guid PostActionId { get; set; }
        public int PostActionType { get; set; }
        public decimal? BonusPoint { get; set; }
        public bool DelFlg { get; set; }
        public DateTime InsDate { get; set; }
        public DateTime UpdDate { get; set; }
        public Guid? GiftVoucherGroupId { get; set; }
        public string Name { get; set; }
        public Guid? BrandId { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<PostActionProductMapping> PostActionProductMapping { get; set; }
        public virtual ICollection<PromotionTier> PromotionTier { get; set; }
        public virtual ICollection<VoucherGroup> VoucherGroup { get; set; }
    }
}
