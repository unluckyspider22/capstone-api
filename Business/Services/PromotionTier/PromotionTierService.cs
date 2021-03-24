
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class PromotionTierService : BaseService<PromotionTier, PromotionTierDto>, IPromotionTierService
    {
        public PromotionTierService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        protected override IGenericRepository<PromotionTier> _repository => _unitOfWork.PromotionTierRepository;

        public async Task<bool> AssignTierToPromo(Guid promotionId, Guid promotionTierId)
        {
            try
            {
                var tier = await _repository.GetFirst(filter: o => o.PromotionTierId.Equals(promotionTierId)
                        && (o.PromotionId == null || o.PromotionId.Equals(Guid.Empty)
                        ));
                if (tier != null)
                {
                    IGenericRepository<Promotion> promoRepo = _unitOfWork.PromotionRepository;
                    var now = DateTime.Now;
                    var promo = await promoRepo.GetFirst(filter: o => o.PromotionId.Equals(promotionId) && !o.DelFlg);
                    if (promo != null)
                    {

                        promo.PromotionTier.Add(tier);
                        promo.UpdDate = now;
                        promoRepo.Update(promo);

                        tier.Promotion = promo;
                        tier.PromotionId = promo.PromotionId;
                        tier.TierIndex = promo.PromotionTier.Count();
                        tier.UpdDate = now;
                        _repository.Update(tier);
                    }
                }
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.InnerException);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }

        }

        public async Task<GenericRespones<AvailableDto>> GetAvailable(int pageSize, int pageIndex, Guid brandId, string actionType, string discountType)
        {
            try
            {
                var result = new List<AvailableDto>();
                var tiers = new List<PromotionTier>();
                if (actionType.Equals(AppConstant.EnvVar.ActionType.Product) || actionType.Equals(AppConstant.EnvVar.ActionType.Order))
                {
                    #region Action
                    tiers = (await _repository.Get(pageIndex: pageIndex, pageSize: pageSize,
                        filter: o => o.PromotionId.Equals(Guid.Empty)
                            || o.PromotionId == null,
                        includeProperties: "ConditionRule,Action"
                     )).Where(o => o.Action != null 
                            && o.Action.ActionType.Equals(actionType.ToString())
                            && o.Action.DiscountType.Equals(discountType.ToString())
                            && o.ConditionRule.BrandId.Equals(brandId)).ToList();
                    foreach (var tier in tiers)
                    {
                        var obj = new AvailableDto()
                        {
                            PromotionTierId = tier.PromotionTierId,
                            RuleName = tier.ConditionRule.RuleName,
                            Description = tier.ConditionRule.Description,
                            ActionType = tier.Action.ActionType,
                            DiscountType = tier.Action.DiscountType,
                        };
                        result.Add(obj);

                    }
                    var totalItem = (await _repository.Get(filter: o => o.PromotionId.Equals(Guid.Empty)
                            || o.PromotionId == null,
                        includeProperties: "ConditionRule,Action"
                     )).Where(o => o.Action != null 
                            && o.Action.ActionType.Equals(actionType.ToString())
                            && o.Action.DiscountType.Equals(discountType.ToString())
                            && o.ConditionRule.BrandId.Equals(brandId)).ToList().Count();
                    MetaData metadata = new MetaData(pageIndex: pageIndex, pageSize: pageSize, totalItems: totalItem);
                    GenericRespones<AvailableDto> response = new GenericRespones<AvailableDto>(data: result, metadata: metadata);
                    return response;
                    #endregion
                }
                else
                {
                    #region Post Action
                    tiers = (await _repository.Get(pageIndex: pageIndex, pageSize: pageSize,
                        filter: o => o.PromotionId.Equals(Guid.Empty)
                            || o.PromotionId == null,
                        includeProperties: "ConditionRule,PostAction"
                     )).Where(o => o.PostAction != null
                            && o.PostAction.ActionType.Equals(actionType.ToString())
                            && o.PostAction.DiscountType.Equals(discountType.ToString())
                            && o.ConditionRule.BrandId.Equals(brandId)).ToList();
                    foreach (var tier in tiers)
                    {
                        var obj = new AvailableDto()
                        {
                            PromotionTierId = tier.PromotionTierId,
                            RuleName = tier.ConditionRule.RuleName,
                            Description = tier.ConditionRule.Description,
                            ActionType = tier.PostAction.ActionType,
                            DiscountType = tier.PostAction.DiscountType,
                        };
                        result.Add(obj);


                    }
                    var totalItem = (await _repository.Get(filter: o => o.PromotionId.Equals(Guid.Empty)
                            || o.PromotionId == null,
                        includeProperties: "ConditionRule,PostAction"
                     )).Where(o => o.PostAction != null 
                            && o.PostAction.ActionType.Equals(actionType.ToString())
                            && o.PostAction.DiscountType.Equals(discountType.ToString())
                            && o.ConditionRule.BrandId.Equals(brandId)).ToList().Count();
                    MetaData metadata = new MetaData(pageIndex: pageIndex, pageSize: pageSize, totalItems: totalItem);
                    GenericRespones<AvailableDto> response = new GenericRespones<AvailableDto>(data: result, metadata: metadata);
                    return response;
                    #endregion
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }

        }
    }

}

