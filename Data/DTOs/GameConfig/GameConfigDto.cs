using System;
using System.Collections.Generic;

namespace Infrastructure.DTOs
{
    public class GameConfigDto : BaseDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Guid GameMasterId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; } = 1;
        public DateTime? StartGame { get; set; }
        public DateTime? EndGame { get; set; }
        public Guid? PromotionId { get; set; }
        public virtual ICollection<GameItemDto> GameItems { get; set; }
    }
}
