
using ApplicationCore.Request;
using Infrastructure.DTOs;
using Infrastructure.DTOs.Voucher;
using Infrastructure.DTOs.VoucherChannel;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IPromotionService : IBaseService<Promotion, PromotionDto>
    {
        Task<List<PromotionTierResponseParam>> GetPromotionTierDetail(Guid promotionId);
        Task<PromotionTierParam> CreatePromotionTier(PromotionTierParam promotionTierParam);
        Task<OrderResponseModel> HandlePromotion(OrderResponseModel orderResponse);
        Task<bool> DeletePromotionTier(DeleteTierRequestParam deleteTierRequestParam);
        Task<PromotionTierUpdateParam> UpdatePromotionTier(PromotionTierUpdateParam updateParam);
        Task<PromotionDto> UpdatePromotion(PromotionDto dto);

    }
}
