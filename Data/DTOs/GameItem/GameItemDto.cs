using System;

namespace Infrastructure.DTOs
{
    public class GameItemDto
    {
        public Guid Id { get; set; }
        public Guid PromotionId { get; set; }
        public Guid GameId { get; set; }
        public int Priority { get; set; }
        public string DisplayText { get; set; }
        public string Description { get; set; }
    }
}
