﻿using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class Promotion
    {
        public Promotion()
        {
            PromotionStoreMapping = new HashSet<PromotionStoreMapping>();
            PromotionTier = new HashSet<PromotionTier>();
            VoucherChannel = new HashSet<VoucherChannel>();
            VoucherGroup = new HashSet<VoucherGroup>();
        }

        public Guid PromotionId { get; set; }
        public Guid? VoucherGroupId { get; set; }
        public Guid? BrandId { get; set; }
        public string PromotionName { get; set; }
        public string PromotionType { get; set; }
        public string ActionType { get; set; }
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
        public bool? IsLimitInDay { get; set; }
        public decimal? LimitCount { get; set; }
        public bool? IsForStore { get; set; }
        public string DayFilter { get; set; }
        public string HourFilter { get; set; }
        public string Rank { get; set; }
        public string Status { get; set; }
        public string IsActive { get; set; }
        public string DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<PromotionStoreMapping> PromotionStoreMapping { get; set; }
        public virtual ICollection<PromotionTier> PromotionTier { get; set; }
        public virtual ICollection<VoucherChannel> VoucherChannel { get; set; }
        public virtual ICollection<VoucherGroup> VoucherGroup { get; set; }
    }
}
