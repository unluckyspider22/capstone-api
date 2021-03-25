using System;

namespace Infrastructure.DTOs
{
    public class GameConfigDto : BaseDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Guid GameMasterId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
