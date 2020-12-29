using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IPromotionStoreMappingService
    {
        public List<Infrastructure.Models.PromotionStoreMapping> GetPromotionStoreMappings();

        public Infrastructure.Models.PromotionStoreMapping GetPromotionStoreMapping(Guid id);

        public int PostPromotionStoreMapping(PromotionStoreMapping promotionStoreMapping);

        public int PutPromotionStoreMapping(PromotionStoreMapping promotionStoreMapping);

        public int DeletePromotionStoreMapping(Guid id);
    }
}
