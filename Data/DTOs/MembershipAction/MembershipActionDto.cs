using System;

namespace Infrastructure.DTOs
{
    public class MembershipActionDto : BaseDto
    {
        public Guid MembershipActionId { get; set; }
        public string GroupNo { get; set; }
        public Guid? PromotionTierId { get; set; }
        public string ActionType { get; set; }
        public string DiscountType { get; set; }
        public string GiftProductCode { get; set; }
        public string GiftName { get; set; }
        public string GiftVoucherCode { get; set; }
        public decimal? BonusPoint { get; set; }
    }
}
