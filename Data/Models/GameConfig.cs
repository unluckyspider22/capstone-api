using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class GameConfig
    {
        public GameConfig()
        {
            GameItems = new HashSet<GameItems>();
        }

        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Guid GameMasterId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public bool DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime UpdDate { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<GameItems> GameItems { get; set; }
    }
}
