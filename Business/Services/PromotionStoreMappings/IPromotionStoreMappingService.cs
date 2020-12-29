using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IPromotionStoreMappingService
    {
        public List<PromotionStoreMapping> GetPromotionStoreMappings();

        public PromotionStoreMapping GetPromotionStoreMapping(Guid id);

        public int PostPromotionStoreMapping(PromotionStoreMapping promotionStoreMapping);

        public int PutPromotionStoreMapping(PromotionStoreMapping promotionStoreMapping);

        public int DeletePromotionStoreMapping(Guid id);
    }
}
