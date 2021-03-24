using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class GameItems
    {
        public GameItems()
        {
            GamePromoMapping = new HashSet<GamePromoMapping>();
        }

        public Guid Id { get; set; }
        public Guid? GameId { get; set; }
        public int? Priority { get; set; }
        public string DisplayText { get; set; }
        public string Description { get; set; }
        public bool? DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual Game Game { get; set; }
        public virtual ICollection<GamePromoMapping> GamePromoMapping { get; set; }
    }
}
