﻿
using ApplicationCore.Utils;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e.Message);
            }

        }

        public async Task<PromotionTierDto> CreateTier(PromotionTierDto dto)
        {
            dto.PromotionTierId = Guid.NewGuid();
            dto.InsDate = DateTime.Now;
            dto.UpdDate = DateTime.Now;
            var entity = _mapper.Map<PromotionTier>(dto);
            _repository.Add(entity);
            if (dto.VoucherGroupId != null && !dto.VoucherGroupId.Equals(Guid.Empty))
            {
                await UpdateVoucher(dto);
            }
            await _unitOfWork.SaveAsync();
            return _mapper.Map<PromotionTierDto>(entity);

        }
        private async Task UpdateVoucher(PromotionTierDto dto)
        {
            try
            {
                IGenericRepository<Voucher> voucherRepo = _unitOfWork.VoucherRepository;
                IGenericRepository<Promotion> promotionRepo = _unitOfWork.PromotionRepository;
                var voucherGroupId = dto.VoucherGroupId;

                var vouchers = await voucherRepo.Get(filter: o => o.VoucherGroupId.Equals(voucherGroupId));
                if (vouchers.Count() > 0 && dto.VoucherQuantity > 0)
                {
                    var promotion = await promotionRepo.GetFirst(filter: o => o.PromotionId.Equals(dto.PromotionId) && !o.DelFlg);
                    var remain = dto.VoucherQuantity;
                    while (remain > 0)
                    {
                        var voucher = vouchers.Where(o => (o.PromotionTierId == null || o.PromotionTierId.Equals(Guid.Empty))
                                            && (o.PromotionId == null || o.PromotionId.Equals(Guid.Empty))).First();
                        if (voucher != null)
                        {
                            voucher.PromotionTierId = dto.PromotionTierId;
                            voucher.PromotionId = dto.PromotionId;
                            voucher.Promotion = promotion;
                            voucher.UpdDate = DateTime.Now;
                            promotion.Voucher.Add(voucher);
                            voucherRepo.Update(voucher);
                        }
                        if (voucher == null && remain > 0)
                        {
                            remain = 0;
                        }
                        else
                        {
                            remain--;
                        }
                    }

                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e.Message);
            }

        }

        public async Task<GenericRespones<AvailableDto>> GetAvailable(int pageSize, int pageIndex, Guid brandId, string actionType, string discountType)
        {
            try
            {
                /* var result = new List<AvailableDto>();
                 var tiers = new List<PromotionTier>();
                 if (actionType.Equals(AppConstant.EnvVar.ActionType.Product) || actionType.Equals(AppConstant.EnvVar.ActionType.Order))
                 {
                     #region Action
                     //tiers = (await _repository.Get(pageIndex: pageIndex, pageSize: pageSize,
                     //    filter: o => o.PromotionId.Equals(Guid.Empty)
                     //        || o.PromotionId == null,
                     //    includeProperties: "ConditionRule,Action"
                     // )).Where(o => o.Action != null 
                     //        && o.Action.ActionType.Equals(actionType.ToString())
                     //        && o.Action.DiscountType.Equals(discountType.ToString())
                     //        && o.ConditionRule.BrandId.Equals(brandId)).ToList();
                     //foreach (var tier in tiers)
                     //{
                     //    var obj = new AvailableDto()
                     //    {
                     //        PromotionTierId = tier.PromotionTierId,
                     //        RuleName = tier.ConditionRule.RuleName,
                     //        Description = tier.ConditionRule.Description,
                     //        ActionType = tier.Action.ActionType,
                     //        DiscountType = tier.Action.DiscountType,
                     //    };
                     //    result.Add(obj);

                     //}
                     //var totalItem = (await _repository.Get(filter: o => o.PromotionId.Equals(Guid.Empty)
                     //        || o.PromotionId == null,
                     //    includeProperties: "ConditionRule,Action"
                     // )).Where(o => o.Action != null 
                     //        && o.Action.ActionType.Equals(actionType.ToString())
                     //        && o.Action.DiscountType.Equals(discountType.ToString())
                     //        && o.ConditionRule.BrandId.Equals(brandId)).ToList().Count();
                     //MetaData metadata = new MetaData(pageIndex: pageIndex, pageSize: pageSize, totalItems: totalItem);
                     //GenericRespones<AvailableDto> response = new GenericRespones<AvailableDto>(data: result, metadata: metadata);
                     //return response;
                     #endregion
                 }
                 else
                 {
                     #region Post Action
                     //tiers = (await _repository.Get(pageIndex: pageIndex, pageSize: pageSize,
                     //    filter: o => o.PromotionId.Equals(Guid.Empty)
                     //        || o.PromotionId == null,
                     //    includeProperties: "ConditionRule,Gift"
                     // )).Where(o => o.Gift != null
                     //        && o.Gift.ActionType.Equals(actionType.ToString())
                     //        && o.Gift.DiscountType.Equals(discountType.ToString())
                     //        && o.ConditionRule.BrandId.Equals(brandId)).ToList();
                     //foreach (var tier in tiers)
                     //{
                     //    var obj = new AvailableDto()
                     //    {
                     //        PromotionTierId = tier.PromotionTierId,
                     //        RuleName = tier.ConditionRule.RuleName,
                     //        Description = tier.ConditionRule.Description,
                     //        ActionType = tier.Gift.ActionType,
                     //        DiscountType = tier.Gift.DiscountType,
                     //    };
                     //    result.Add(obj);


                     //}
                     //var totalItem = (await _repository.Get(filter: o => o.PromotionId.Equals(Guid.Empty)
                     //        || o.PromotionId == null,
                     //    includeProperties: "ConditionRule,Gift"
                     // )).Where(o => o.Gift != null 
                     //        && o.Gift.ActionType.Equals(actionType.ToString())
                     //        && o.Gift.DiscountType.Equals(discountType.ToString())
                     //        && o.ConditionRule.BrandId.Equals(brandId)).ToList().Count();
                     //MetaData metadata = new MetaData(pageIndex: pageIndex, pageSize: pageSize, totalItems: totalItem);
                     //GenericRespones<AvailableDto> response = new GenericRespones<AvailableDto>(data: result, metadata: metadata);
                     //return response;
                     #endregion
                 }*/
                return null;
            }

            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e.Message);
            }

        }

        public async Task<bool> DeleteTier(Guid id)
        {
            try
            {
                IVoucherRepository myRepo = new VoucherRepositoryImp();
                IGenericRepository<VoucherGroup> groupRepo = _unitOfWork.VoucherGroupRepository;
                var tierEntity = await _repository.GetFirst(filter: el => el.PromotionTierId.Equals(id));
                if (tierEntity != null)
                {
                    var groupId = tierEntity.VoucherGroupId;
                    if (groupId != null && !groupId.Equals(Guid.Empty))
                    {
                        await myRepo.UpdateVoucherGroupWhenDeletetier(voucherGroupId: (Guid)groupId, tierId: tierEntity.PromotionTierId);
                    }
                }
                _repository.Delete(id: id);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new ErrorObj(code: 500, message: e.Message);
            }

        }
        #region Update promotion tier
        public async Task<bool> UpdateTier(PromotionTierDto dto, PromotionTier entity)
        {
            try
            {
                var now = Common.GetCurrentDatetime();
                if (dto.VoucherGroupId != null && !dto.VoucherGroupId.Equals(Guid.Empty))
                {
                    entity = await UpdateVoucherOfTier(dto: dto, entity: entity);
                }

                if (dto.ConditionRuleId != null && !dto.ConditionRuleId.Equals(Guid.Empty))
                {
                    entity = await UpdateConditionOfTier(dto: dto, entity: entity);
                }
                if (dto.ActionId != null && !dto.ActionId.Equals(Guid.Empty))
                {
                    entity = await UpdateActionOfTier(dto: dto, entity: entity);
                }
                if (dto.GiftId != null && !dto.GiftId.Equals(Guid.Empty))
                {
                    entity = await UpdateGiftOfTier(dto: dto, entity: entity);
                }
                entity.UpdDate = now;
                entity.Priority = dto.Priority;
                _repository.Update(entity);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e.Message);
            }

        }

        private async Task<PromotionTier> UpdateVoucherOfTier(PromotionTierDto dto, PromotionTier entity)
        {
            try
            {
                var now = Common.GetCurrentDatetime();
                var oldId = entity.VoucherGroupId;

                // Trường hợp đổi voucher group
                if (!dto.VoucherGroupId.Equals(oldId))
                {
                    IVoucherRepository myRepo = new VoucherRepositoryImp();
                    await myRepo.UpdateVoucherGroupWhenDeletetier(voucherGroupId: (Guid)oldId, tierId: dto.PromotionTierId);
                    await UpdateVoucher(dto);
                    IGenericRepository<VoucherGroup> groupRepo = _unitOfWork.VoucherGroupRepository;
                    var voucherGroup = await groupRepo.GetFirst(filter: el => el.VoucherGroupId.Equals(dto.VoucherGroupId));
                    if (voucherGroup != null)
                    {
                        entity.VoucherGroup = voucherGroup;
                        entity.VoucherGroupId = voucherGroup.VoucherGroupId;
                    }
                }
                else
                {
                    // Trường hợp tạo thêm voucher
                    if (dto.MoreQuantity > 0)
                    {
                        IGenericRepository<Voucher> voucherRepo = _unitOfWork.VoucherRepository;
                        var vouchers = await voucherRepo.Get(filter: el => el.VoucherGroupId.Equals(dto.VoucherGroupId)
                                                                    && (el.PromotionId.Equals(Guid.Empty) || el.PromotionId == null)
                                                                    && (el.PromotionTierId.Equals(Guid.Empty) || el.PromotionTierId == null)
                                                                    && !el.VoucherGroup.DelFlg);
                        if (vouchers.Count() > 0)
                        {
                            var remain = dto.MoreQuantity;
                            while (remain > 0)
                            {
                                var voucher = vouchers.Where(el => (el.PromotionId.Equals(Guid.Empty) || el.PromotionId == null)
                                                             && (el.PromotionTierId.Equals(Guid.Empty) || el.PromotionTierId == null)).FirstOrDefault();
                                if (voucher != null)
                                {
                                    voucher.PromotionId = dto.PromotionId;
                                    voucher.PromotionTierId = dto.PromotionTierId;
                                    voucher.UpdDate = now;
                                    voucherRepo.Update(voucher);
                                }
                                if (voucher == null && remain > 0)
                                {
                                    remain = 0;
                                }
                                else
                                {
                                    remain--;
                                }
                            }
                            entity.VoucherQuantity += dto.MoreQuantity;
                        }
                    }
                }
                await _unitOfWork.SaveAsync();
                return entity;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e.Message);
            }

        }
        private async Task<PromotionTier> UpdateConditionOfTier(PromotionTierDto dto, PromotionTier entity)
        {
            try
            {
                var oldId = entity.ConditionRuleId;
                var newId = dto.ConditionRuleId;
                if (!newId.Equals(oldId))
                {
                    IGenericRepository<ConditionRule> conditionRepo = _unitOfWork.ConditionRuleRepository;
                    var newCondition = await conditionRepo.GetFirst(filter: el => el.ConditionRuleId.Equals(newId) && !el.DelFlg);
                    if (newCondition != null)
                    {
                        entity.ConditionRule = newCondition;
                        entity.ConditionRuleId = newCondition.ConditionRuleId;
                    }
                    else
                    {
                        throw new ErrorObj(404, "Cannot find condition");
                    }
                }
                return entity;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e.Message);
            }
        }
        private async Task<PromotionTier> UpdateActionOfTier(PromotionTierDto dto, PromotionTier entity)
        {
            try
            {
                var oldId = entity.ActionId;
                var newId = dto.ActionId;
                if (!newId.Equals(oldId))
                {
                    IGenericRepository<Infrastructure.Models.Action> actionRepo = _unitOfWork.ActionRepository;
                    var newAction = await actionRepo.GetFirst(filter: el => el.ActionId.Equals(newId) && !el.DelFlg);
                    if (newAction != null)
                    {
                        entity.Action = newAction;
                        entity.ActionId = newAction.ActionId;
                    }
                    else
                    {
                        throw new ErrorObj(404, "Cannot find action");
                    }
                }
                return entity;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e.Message);
            }
        }
        private async Task<PromotionTier> UpdateGiftOfTier(PromotionTierDto dto, PromotionTier entity)
        {
            try
            {
                var oldId = entity.GiftId;
                var newId = dto.GiftId;
                if (!newId.Equals(oldId))
                {
                    IGenericRepository<Gift> actionRepo = _unitOfWork.GiftRepository;
                    var newAction = await actionRepo.GetFirst(filter: el => el.GiftId.Equals(newId) && !el.DelFlg);
                    if (newAction != null)
                    {
                        entity.Gift = newAction;
                        entity.GiftId = newAction.GiftId;
                    }
                    else
                    {
                        throw new ErrorObj(404, "Cannot find gift");
                    }
                }
                return entity;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e.Message);
            }
        }
        #endregion
    }

}

