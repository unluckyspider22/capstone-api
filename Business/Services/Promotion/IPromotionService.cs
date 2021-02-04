
using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IPromotionService : IBaseService<Promotion, PromotionDto>
    {
        Task<List<PromotionTier>> GetPromotionTierDetail(Guid promotionId);
        Task<PromotionTierParam> CreatePromotionTier(PromotionTierParam promotionTierParam);
        Task<bool> DeletePromotionTier(DeleteTierRequestParam deleteTierRequestParam);
    }
}
