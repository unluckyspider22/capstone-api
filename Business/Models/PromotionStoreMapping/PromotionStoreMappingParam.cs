using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Models.PromotionStoreMapping
{
    public class PromotionStoreMappingParam
    {
        public Guid Id { get; set; }
        public Guid? StoreId { get; set; }

        public Guid? PromotionId { get; set; }
    }
}
