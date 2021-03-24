using System;

namespace Infrastructure.DTOs
{
    public class GameDto
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string Name { get; set; }
        public int MinItem { get; set; } = 1;
        public int MaxItem { get; set; } = 1;
    }
}
