using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class PostActionProductMapping
    {
        public Guid Id { get; set; }
        public Guid PostActionId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime InsDate { get; set; }
        public DateTime UpdDate { get; set; }
        public int? Quantity { get; set; }

        public virtual PostAction PostAction { get; set; }
        public virtual Product Product { get; set; }
    }
}
