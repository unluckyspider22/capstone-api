using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class ActionDto : BaseDto
    {
        public Guid ActionId { get; set; }
        public Guid PromotionTierId { get; set; }
        public string ActionType { get; set; }
        public string DiscountType { get; set; }
        public string ParentCode { get; set; }
        public string ProductCode { get; set; }
        public decimal? DiscountQuantity { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? FixedPrice { get; set; }
        public decimal? MaxAmount { get; set; }
        public decimal? MinPriceAfter { get; set; }
        public decimal? OrderLadderProduct { get; set; }
        public decimal? LadderPrice { get; set; }
        public decimal? BundlePrice { get; set; }
        public decimal? BundleQuantity { get; set; }
        public string BundleStrategy { get; set; }
    }
}
