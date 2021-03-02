using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs
{
    public class PromotionDto : BaseDto
    {
        public Guid PromotionId { get; set; }
        public Guid? VoucherGroupId { get; set; }
        public Guid? BrandId { get; set; }
        [StringLength(6)]
        public string PromotionCode { get; set; }
        [StringLength(100)]
        public string PromotionName { get; set; }
        [StringLength(1)]
        public string PromotionType { get; set; }
        [StringLength(1)]
        public string ActionType { get; set; }
        [StringLength(1)]
        public string DiscountType { get; set; }
        [StringLength(1)]
        public string PromotionLevel { get; set; }
        [StringLength(2048)]
        public string ImgUrl { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [StringLength(1)]
        public string Exclusive { get; set; }
        [StringLength(1)]
        public string ApplyBy { get; set; }
        [StringLength(1)]
        public string SaleMode { get; set; }
        [StringLength(1)]
        public string Gender { get; set; }
        [StringLength(10)]
        public string PaymentMethod { get; set; }
        [StringLength(1)]
        public string ForHoliday { get; set; }
        [StringLength(1)]
        public string ForMembership { get; set; }
        public bool IsForStore { get; set; }
        [StringLength(10)]
        public string DayFilter { get; set; }
        [StringLength(10)]
        public string HourFilter { get; set; }
        [StringLength(3)]
        public string Rank { get; set; }
        [StringLength(1)]
        public string Status { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<PromotionStoreMappingDto> PromotionStoreMapping { get; set; }
    }
}
