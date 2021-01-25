using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class ActionDto : BaseDto
    {
        public Guid ActionId { get; set; }
        public Guid? PromotionTierId { get; set; }
        public string ActionType { get; set; }
        public bool IsLimitAmount { get; set; }
        public string ProductCode { get; set; }
        public decimal? DiscountQuantity { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? FixedPrice { get; set; }
        public decimal? MaxAmount { get; set; }
        public bool ApplyLadderPrice { get; set; }
    }
}
