using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class Promotion
    {
        public Promotion()
        {
            GamePromoMapping = new HashSet<GamePromoMapping>();
            MemberLevelMapping = new HashSet<MemberLevelMapping>();
            PromotionChannelMapping = new HashSet<PromotionChannelMapping>();
            PromotionStoreMapping = new HashSet<PromotionStoreMapping>();
            PromotionTier = new HashSet<PromotionTier>();
            Transaction = new HashSet<Transaction>();
        }

        public Guid PromotionId { get; set; }
        public Guid? BrandId { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionName { get; set; }
        public string PromotionType { get; set; }
        public string ActionType { get; set; }
        public string DiscountType { get; set; }
        public string PromotionLevel { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Exclusive { get; set; }
        public string ApplyBy { get; set; }
        public string SaleMode { get; set; }
        public string Gender { get; set; }
        public string PaymentMethod { get; set; }
        public string ForHoliday { get; set; }
        public string ForMembership { get; set; }
        public bool? IsForStore { get; set; }
        public string DayFilter { get; set; }
        public string HourFilter { get; set; }
        public string Rank { get; set; }
        public string Status { get; set; }
        public bool DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }
        public bool IsForGame { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual VoucherGroup VoucherGroup { get; set; }
        public virtual ICollection<GamePromoMapping> GamePromoMapping { get; set; }
        public virtual ICollection<MemberLevelMapping> MemberLevelMapping { get; set; }
        public virtual ICollection<PromotionChannelMapping> PromotionChannelMapping { get; set; }
        public virtual ICollection<PromotionStoreMapping> PromotionStoreMapping { get; set; }
        public virtual ICollection<PromotionTier> PromotionTier { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
