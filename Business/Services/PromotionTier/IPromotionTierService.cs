using ApplicationCore.Models.PromotionTier;
using Infrastructure.DTOs.PromotionTier;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IPromotionTierService : IBaseService<PromotionTier, PromotionTierDto>
    {
        
    }
}
