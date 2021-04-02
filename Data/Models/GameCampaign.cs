using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class GameCampaign
    {
        public GameCampaign()
        {
            GameItems = new HashSet<GameItems>();
            PostAction = new HashSet<PostAction>();
            Voucher = new HashSet<Voucher>();
        }

        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public Guid GameMasterId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public bool DelFlg { get; set; }
        public DateTime InsDate { get; set; }
        public DateTime UpdDate { get; set; }
        public DateTime? StartGame { get; set; }
        public DateTime? EndGame { get; set; }
        public Guid? PromotionId { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual GameMaster GameMaster { get; set; }
        public virtual Promotion Promotion { get; set; }
        public virtual ICollection<GameItems> GameItems { get; set; }
        public virtual ICollection<PostAction> PostAction { get; set; }
        public virtual ICollection<Voucher> Voucher { get; set; }
    }
}
