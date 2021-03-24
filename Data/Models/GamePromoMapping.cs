using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class GamePromoMapping
    {
        public Guid GameId { get; set; }
        public Guid ItemId { get; set; }
        public Guid PromotionId { get; set; }

        public virtual Game Game { get; set; }
        public virtual GameItems Item { get; set; }
        public virtual Promotion Promotion { get; set; }
    }
}
