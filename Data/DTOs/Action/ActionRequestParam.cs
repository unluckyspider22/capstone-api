using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class ActionRequestParam
    {
        [StringLength(1)]
        public string ActionType { get; set; }
        [StringLength(1)]
        public string DiscountType { get; set; }
        [StringLength(1)]
        public string ProductType { get; set; }
        [StringLength(50)]
        public string ParentCode { get; set; }
        [StringLength(200)]
        public string ProductCode { get; set; }
        [Range(0,999999)]
        public decimal? DiscountQuantity { get; set; }
        [Range(0, 9999999999)]
        public decimal? DiscountAmount { get; set; }
        [Range(0.00, 100.00)]
        public decimal? DiscountPercentage { get; set; }
        [Range(0, 9999999999)]
        public decimal? FixedPrice { get; set; }
        [Range(0, 9999999999)]
        public decimal? MaxAmount { get; set; }
        [Range(0, 9999999999)]
        public decimal? MinPriceAfter { get; set; }
        [Range(0, 9999999999)]
        public decimal? OrderLadderProduct { get; set; }
        [Range(0, 9999999999)]
        public decimal? LadderPrice { get; set; }
        [Range(0, 9999999999)]
        public decimal? BundlePrice { get; set; }
        [Range(0, 999999)]
        public decimal? BundleQuantity { get; set; }
        [StringLength(1)]
        public string BundleStrategy { get; set; }
    }
}
