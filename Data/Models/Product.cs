using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class Product
    {
        public Product()
        {
            ActionProductMapping = new HashSet<ActionProductMapping>();
            PostActionProductMapping = new HashSet<PostActionProductMapping>();
        }

        public Guid ProductId { get; set; }
        public Guid ProductCateId { get; set; }
        public string CateId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool DelFlg { get; set; }
        public DateTime InsDate { get; set; }
        public DateTime UpdDate { get; set; }

        public virtual ProductCategory ProductCate { get; set; }
        public virtual ICollection<ActionProductMapping> ActionProductMapping { get; set; }
        public virtual ICollection<PostActionProductMapping> PostActionProductMapping { get; set; }
    }
}
