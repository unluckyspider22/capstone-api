using ApplicationCore.Models.PromotionTier;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IPromotionTierService
    {
        public List<PromotionTier> GetPromotionTiers();

        public PromotionTier GetPromotionTier(Guid id);

        public int PostPromotionTier(PromotionTier PromotionTier);

        public int PutPromotionTier(Guid id, PromotionTierParam PromotionTierParam);

        public int DeletePromotionTier(Guid id);

        public int CountPromotionTier();

        public int UpdateDelFlag(Guid id, string delflg);
    }
}
