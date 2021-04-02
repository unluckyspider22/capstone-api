using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class GameItems
    {
        public Guid Id { get; set; }
        public Guid PromotionId { get; set; }
        public Guid GameId { get; set; }
        public int Priority { get; set; }
        public string DisplayText { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public bool DelFlg { get; set; }
        public DateTime InsDate { get; set; }
        public DateTime UpdDate { get; set; }

        public virtual GameCampaign Game { get; set; }
        public virtual Promotion Promotion { get; set; }
    }
}
