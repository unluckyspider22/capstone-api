using System;

namespace Infrastructure.DTOs
{
    public class GameItemDto : BaseDto
    {
        public Guid Id { get; set; }
        public Guid PromotionTierId { get; set; }
        public Guid GameId { get; set; }
        public int Priority { get; set; }
        public string ImgUrl { get; set; }

        public decimal Ratio { get; set; }
        public string DisplayText { get; set; }
        public string Description { get; set; }
    }
}
