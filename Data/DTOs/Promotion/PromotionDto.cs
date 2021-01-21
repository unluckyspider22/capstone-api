using Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace Infrastructure.DTOs
{
    public class PromotionDto : BaseDto
    {
        public Guid PromotionId { get; set; }
        public Guid? BrandId { get; set; }
        public string PromotionName { get; set; }
        public string PromotionType { get; set; }
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
        public string IsLimitInDay { get; set; }
        public decimal? LimitCount { get; set; }
        public string IsForStore { get; set; }
        public string DayFilter { get; set; }
        public string HourFilter { get; set; }
        public string Rank { get; set; }
        public string Status { get; set; }
        public string IsActive { get; set; }

        public virtual ICollection<PromotionStoreMappingDto> PromotionStoreMapping { get; set; }
    }
}
