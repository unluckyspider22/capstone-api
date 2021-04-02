using System;
using System.Collections.Generic;

namespace Infrastructure.DTOs
{
    public class PostActionDto : BaseDto
    {
        public Guid PostActionId { get; set; }
        public int ActionType { get; set; }
        public int DiscountType { get; set; }
        public decimal? GiftQuantity { get; set; }
        public decimal? BonusPoint { get; set; }
        public Guid? GiftPromotionId { get; set; }
        public string Name { get; set; }
        public Guid BrandId { get; set; }
        public List<PostActionProductMapp> ListProduct { get; set; }
    }

    public class PostActionProductMapp
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
