using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class Game
    {
        public Game()
        {
            GameItems = new HashSet<GameItems>();
            GamePromoMapping = new HashSet<GamePromoMapping>();
        }

        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string Name { get; set; }
        public int? MinItem { get; set; }
        public int? MaxItem { get; set; }
        public bool? DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<GameItems> GameItems { get; set; }
        public virtual ICollection<GamePromoMapping> GamePromoMapping { get; set; }
    }
}
