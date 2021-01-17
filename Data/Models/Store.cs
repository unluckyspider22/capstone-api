using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class Store
    {
        public Store()
        {
            PromotionStoreMapping = new HashSet<PromotionStoreMapping>();
        }

        public Guid StoreId { get; set; }
        public Guid? BrandId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }
        
        public virtual Brand Brand { get; set; }
        public virtual ICollection<PromotionStoreMapping> PromotionStoreMapping { get; set; }
    }
}
