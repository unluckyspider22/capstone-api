﻿
using ApplicationCore.Chain;
using ApplicationCore.Request;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class PromotionService : BaseService<Promotion, PromotionDto>, IPromotionService
    {
        private readonly IApplyPromotionHandler _applyPromotionHandler;
        private readonly IConditionRuleService _conditionRuleService;
        private readonly IHolidayService _holidayService;
        private readonly ITimeframeHandle _timeframeHandle;

        public PromotionService(IUnitOfWork unitOfWork, IMapper mapper, IApplyPromotionHandler promotionHandle, IConditionRuleService conditionRuleService, IHolidayService holidayService, ITimeframeHandle timeframeHandle) : base(unitOfWork, mapper)
        {
            _applyPromotionHandler = promotionHandle;
            _conditionRuleService = conditionRuleService;
            _holidayService = holidayService;
            _timeframeHandle = timeframeHandle;
        }

        protected override IGenericRepository<Promotion> _repository => _unitOfWork.PromotionRepository;
        #region CRUD promotion tier
        public async Task<PromotionTierParam> CreatePromotionTier(PromotionTierParam param)
        {
            try
            {
                IGenericRepository<ConditionRule> conditionRuleRepo = _unitOfWork.ConditionRuleRepository;
                IGenericRepository<ConditionGroup> conditionGroupRepo = _unitOfWork.ConditionGroupRepository;
                IGenericRepository<ProductCondition> productConditionRepo = _unitOfWork.ProductConditionRepository;
                IGenericRepository<OrderCondition> orderConditionRepo = _unitOfWork.OrderConditionRepository;
                IGenericRepository<MembershipCondition> membershipConditionRepo = _unitOfWork.MembershipConditionRepository;
                IGenericRepository<PromotionTier> promotionTierRepo = _unitOfWork.PromotionTierRepository;
                IGenericRepository<PostAction> postActionRepo = _unitOfWork.PostActionRepository;
                IGenericRepository<Infrastructure.Models.Action> actionRepo = _unitOfWork.ActionRepository;
                // Create condition rule
                var conditionRuleEntity = _mapper.Map<ConditionRule>(param.ConditionRule);
                // Nếu param truyền vào không có condition rule id thì add mới vào db
                if (param.ConditionRule.ConditionRuleId.Equals(Guid.Empty))
                {
                    conditionRuleEntity.ConditionRuleId = Guid.NewGuid();
                    param.ConditionRule.ConditionRuleId = conditionRuleEntity.ConditionRuleId;
                    conditionRuleRepo.Add(conditionRuleEntity);
                }
                else
                {
                    // Nếu đã có condition rule 
                    conditionRuleEntity.UpdDate = DateTime.Now;
                    //conditionRuleEntity.InsDate = null;
                    conditionRuleRepo.Update(conditionRuleEntity);
                    //Delete old condition group of condition rule
                    await DeleteOldGroups(conditionRuleEntity: conditionRuleEntity);
                }
                // Create condition group
                InsertConditionGroup(conditionGroups: param.ConditionGroups, conditionRuleEntity: conditionRuleEntity);
                // Create promotion tier

                PromotionTier promotionTier = new PromotionTier
                {
                    PromotionTierId = Guid.NewGuid(),
                    ConditionRuleId = conditionRuleEntity.ConditionRuleId,
                    InsDate = DateTime.Now,
                    UpdDate = DateTime.Now,
                };
                if (!param.PromotionId.Equals(Guid.Empty))
                {
                    promotionTier.PromotionId = param.PromotionId;
                }
                else
                {
                    promotionTier.PromotionId = null;
                }

                // Create action
                if (param.Action.ActionType != null)
                {

                    var countTier = await promotionTierRepo.CountAsync(filter: o => o.PromotionId.Equals(promotionTier.PromotionId));
                    var actionEntity = _mapper.Map<Infrastructure.Models.Action>(param.Action);
                    actionEntity.ActionId = Guid.NewGuid();
                    actionEntity.PromotionTierId = promotionTier.PromotionTierId;
                    actionEntity.ActionType = actionEntity.ActionType.Trim();
                    actionEntity.DiscountType = actionEntity.DiscountType.Trim();
                    promotionTier.Summary = CreateSummaryAction(actionEntity);
                    promotionTier.ActionId = actionEntity.ActionId;
                    promotionTier.TierIndex = countTier;
                    promotionTierRepo.Add(promotionTier);
                    actionRepo.Add(actionEntity);

                    // Create action product mapping
                    IGenericRepository<ActionProductMapping> mapRepo = _unitOfWork.ActionProductMappingRepository;
                    if (param.Action.ActionType.Equals(AppConstant.EnvVar.ActionType.Product)
                        && param.Action.ListProduct.Count > 0)
                    {
                        foreach (var product in param.Action.ListProduct)
                        {
                            var mappEntity = new ActionProductMapping()
                            {
                                Id = Guid.NewGuid(),
                                ActionId = actionEntity.ActionId,
                                ProductId = product,
                                InsDate = DateTime.Now,
                                UpdDate = DateTime.Now,
                            };
                            mapRepo.Add(mappEntity);
                        }
                        //await _unitOfWork.SaveAsync();
                    }
                    param.Action = _mapper.Map<ActionRequestParam>(actionEntity);
                }
                else
                if (param.PostAction.ActionType != null)
                {
                    // Create membership action

                    var countTier = await promotionTierRepo.CountAsync(filter: o => o.PromotionId.Equals(promotionTier.ActionId));
                    var postAction = _mapper.Map<PostAction>(param.PostAction);
                    postAction.PostActionId = Guid.NewGuid();
                    postAction.PromotionTierId = promotionTier.PromotionTierId;
                    postAction.ActionType = postAction.ActionType.Trim();
                    postAction.DiscountType = postAction.DiscountType.Trim();
                    //promotionTier.Summary = CreateSummarypostAction(postAction);
                    promotionTier.Summary = "";
                    promotionTier.TierIndex = countTier;
                    promotionTier.PostActionId = postAction.PostActionId;

                    promotionTierRepo.Add(promotionTier);
                    postActionRepo.Add(postAction);

                    // Create action product mapping
                    IGenericRepository<PostActionProductMapping> mapRepo = _unitOfWork.PostActionProductMappingRepository;
                    if (param.PostAction.ActionType.Equals(AppConstant.EnvVar.ActionType.Gift)
                        && param.PostAction.DiscountType.Equals(AppConstant.EnvVar.DiscountType.GiftProduct)
                        && param.PostAction.ListProduct.Count > 0)
                    {
                        foreach (var product in param.PostAction.ListProduct)
                        {
                            var mappEntity = new PostActionProductMapping()
                            {
                                Id = Guid.NewGuid(),
                                PostActionId = postAction.PostActionId,
                                ProductId = product,
                                InsDate = DateTime.Now,
                                UpdDate = DateTime.Now,
                            };
                            mapRepo.Add(mappEntity);
                        }
                        //await _unitOfWork.SaveAsync();
                    }
                    param.PostAction = _mapper.Map<PostActionRequestParam>(postAction);
                }
                else
                {
                    throw new ErrorObj(code: 400, message: "Action or Membership action is not null", description: "Invalid param");
                }
                await _unitOfWork.SaveAsync();
                return param;
            }
            catch (ErrorObj e)
            {
                throw e;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.InnerException);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.", description: "Internal Server Error");
            }
        }

        public async Task<bool> DeletePromotionTier(DeleteTierRequestParam param)
        {
            try
            {
                IGenericRepository<PromotionTier> tierRepo = _unitOfWork.PromotionTierRepository;
                var tierEntity = await tierRepo.GetFirst(filter: o => o.PromotionTierId.Equals(param.PromotionTierId));

                var promotions = await _repository.GetFirst(filter: o => o.PromotionId.Equals(param.PromotionId) && !o.DelFlg, includeProperties: "PromotionTier");
                var tiers = promotions.PromotionTier;
                tiers.Remove(tierEntity);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    var promotionTiers = await tierRepo.Get(filter: o => o.PromotionId.Equals(param.PromotionId));
                    for (int i = 0; i < promotionTiers.Count(); i++)
                    {
                        var tier = promotionTiers.ElementAt(i);
                        tier.TierIndex = i;
                        tierRepo.Update(tier);
                    }
                }


                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.InnerException);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.", description: "Internal Server Error");
            }
        }

        public async Task<List<PromotionTierResponseParam>> GetPromotionTierDetail(Guid promotionId)
        {
            IGenericRepository<PromotionTier> _tierRepo = _unitOfWork.PromotionTierRepository;
            IGenericRepository<ActionProductMapping> actionMappRepo = _unitOfWork.ActionProductMappingRepository;
            IGenericRepository<ProductCategory> cateRepo = _unitOfWork.ProductCategoryRepository;
            IGenericRepository<PostActionProductMapping> postActionMappRepo = _unitOfWork.PostActionProductMappingRepository;
            try
            {
                // Lấy danh sách promotion tier
                Expression<Func<PromotionTier, bool>> filter = el => el.PromotionId.Equals(promotionId);
                var tiers = (
                    await _tierRepo.Get(0, 0, filter: filter,
                    orderBy: el => el.OrderBy(o => o.InsDate),
                    includeProperties: "ConditionRule," +
                    "ConditionRule.ConditionGroup," +
                    "ConditionRule.ConditionGroup.OrderCondition," +
                    "ConditionRule.ConditionGroup.ProductCondition," +
                    "PostAction," +
                    "Action"))
                    .ToList();
                // Reorder các condition trong group
                List<PromotionTierResponseParam> result = new List<PromotionTierResponseParam>();
                foreach (var tier in tiers)
                {
                    PromotionTierResponseParam responseParam = new PromotionTierResponseParam
                    {
                        Action = _mapper.Map<ActionTierDto>(tier.Action),
                        ActionId = tier.ActionId,
                        PostAction = _mapper.Map<PostActionTierDto>(tier.PostAction),
                        PostActionId = tier.PostActionId,
                        PromotionId = tier.PromotionId,
                        PromotionTierId = tier.PromotionTierId,
                        ConditionRuleId = tier.ConditionRuleId,
                        ConditionRule = await _conditionRuleService.ReorderResult(tier.ConditionRule),
                        Summary = tier.Summary,
                    };
                    if (responseParam.Action != null)
                    {
                        responseParam.Action.productList = new List<ProductDto>();
                        var mapps = (await actionMappRepo.Get(filter: o => o.ActionId.Equals(responseParam.ActionId),
                            includeProperties: "Product")).ToList();
                        if (mapps != null && mapps.Count > 0)
                        {
                            foreach (var mapp in mapps)
                            {
                                var product = mapp.Product;
                                var cate = await cateRepo.GetFirst(filter: o => o.ProductCateId.Equals(product.ProductCateId)
                                && !o.DelFlg);
                                var dto = new ProductDto()
                                {
                                    CateId = cate != null ? cate.CateId : "",
                                    ProductCateId = cate != null ? cate.ProductCateId : Guid.Empty,
                                    Name = product.Name,
                                    Code = product.Code,
                                    ProductId = product.ProductId
                                };
                                responseParam.Action.productList.Add(dto);
                            }

                        }
                        else
                        {
                            responseParam.Action.productList = new List<ProductDto>();
                        }
                    }
                    else if (responseParam.PostAction != null)
                    {
                        responseParam.PostAction.productList = new List<ProductDto>();
                        var mapps = (await postActionMappRepo.Get(filter: o => o.PostActionId.Equals(responseParam.PostActionId),
                            includeProperties: "Product")).ToList();
                        if (mapps != null && mapps.Count > 0)
                        {
                            foreach (var mapp in mapps)
                            {
                                var product = mapp.Product;
                                var cate = await cateRepo.GetFirst(filter: o => o.ProductCateId.Equals(product.ProductCateId)
                                  && !o.DelFlg);
                                var dto = new ProductDto()
                                {
                                    CateId = cate != null ? cate.CateId : "",
                                    ProductCateId = cate != null ? cate.ProductCateId : Guid.Empty,
                                    Name = product.Name,
                                    Code = product.Code,
                                    ProductId = product.ProductId
                                };
                                responseParam.PostAction.productList.Add(dto);
                            }

                        }
                        else
                        {
                            responseParam.PostAction.productList = new List<ProductDto>();
                        }
                    }
                    result.Add(responseParam);
                }
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.InnerException);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.", description: "Internal Server Error");
            }
        }
        public async Task<PromotionTierUpdateParam> UpdatePromotionTier(PromotionTierUpdateParam updateParam)
        {

            try
            {
                IGenericRepository<PromotionTier> promotionTierRepo = _unitOfWork.PromotionTierRepository;
                // update action
                if (!updateParam.Action.ActionId.Equals(Guid.Empty))
                {
                    var actionEntity = _mapper.Map<Infrastructure.Models.Action>(updateParam.Action);
                    IGenericRepository<Infrastructure.Models.Action> actionRepo = _unitOfWork.ActionRepository;
                    actionEntity.UpdDate = DateTime.Now;
                    actionEntity.InsDate = null;
                    actionEntity.PromotionTierId = updateParam.PromotionTierId;
                    actionRepo.Update(actionEntity);
                    var tier = await promotionTierRepo.GetFirst(filter: el => el.ActionId.Equals(actionEntity.ActionId));
                    tier.Summary = CreateSummaryAction(actionEntity);
                    tier.UpdDate = DateTime.Now;
                    promotionTierRepo.Update(tier);
                    // Update danh sách các product trong bảng map
                    IGenericRepository<ActionProductMapping> actMapp = _unitOfWork.ActionProductMappingRepository;
                    actMapp.Delete(Guid.Empty, filter: o => o.ActionId.Equals(actionEntity.ActionId));
                    await _unitOfWork.SaveAsync();
                    var productIds = updateParam.Action.ListProduct;
                    foreach (var productId in productIds)
                    {
                        var mapp = new ActionProductMapping()
                        {
                            Id = Guid.NewGuid(),
                            ActionId = actionEntity.ActionId,
                            ProductId = productId,
                            InsDate = DateTime.Now,
                            UpdDate = DateTime.Now,
                        };
                        actMapp.Add(mapp);
                    }
                }
                else if (!updateParam.PostAction.PostActionId.Equals(Guid.Empty))
                {
                    var postActionEntity = _mapper.Map<PostAction>(updateParam.PostAction);
                    IGenericRepository<PostAction> postActionRepo = _unitOfWork.PostActionRepository;
                    postActionEntity.UpdDate = DateTime.Now;
                    postActionEntity.InsDate = null;
                    postActionEntity.PromotionTierId = updateParam.PromotionTierId;
                    postActionRepo.Update(postActionEntity);
                    var tier = await promotionTierRepo.GetFirst(filter: el => el.PostActionId.Equals(postActionEntity.PostActionId));
                    //tier.Summary = CreateSummaryPostAction(postActionEntity);
                    tier.UpdDate = DateTime.Now;
                    promotionTierRepo.Update(tier);
                    // Update danh sách các product trong bảng map
                    IGenericRepository<PostActionProductMapping> actMapp = _unitOfWork.PostActionProductMappingRepository;
                    actMapp.Delete(Guid.Empty, filter: o => o.PostActionId.Equals(postActionEntity.PostActionId));
                    await _unitOfWork.SaveAsync();
                    var productIds = updateParam.PostAction.ListProduct;
                    foreach (var productId in productIds)
                    {
                        var mapp = new PostActionProductMapping()
                        {
                            Id = Guid.NewGuid(),
                            PostActionId = postActionEntity.PostActionId,
                            ProductId = productId,
                            InsDate = DateTime.Now,
                            UpdDate = DateTime.Now,
                        };
                        actMapp.Add(mapp);
                    }
                }
                else
                {
                    throw new ErrorObj(code: 400, message: "Action or Membership action is not null", description: "Invalid param");
                }
                //await _unitOfWork.SaveAsync();
                // update condition rule
                if (!updateParam.ConditionRule.ConditionRuleId.Equals(Guid.Empty))
                {
                    IGenericRepository<ConditionRule> conditionRepo = _unitOfWork.ConditionRuleRepository;
                    var conditionRuleEntity = _mapper.Map<ConditionRule>(updateParam.ConditionRule);
                    conditionRuleEntity.UpdDate = DateTime.Now;
                    conditionRepo.Update(conditionRuleEntity);
                    await DeleteOldGroups(conditionRuleEntity: conditionRuleEntity);
                    InsertConditionGroup(conditionGroups: updateParam.ConditionGroups, conditionRuleEntity: conditionRuleEntity);
                }


                await _unitOfWork.SaveAsync();
                return updateParam;
            }
            catch (ErrorObj e)
            {
                throw e;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.InnerException);
                Debug.WriteLine(e.ToString());
                Debug.WriteLine(e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.", description: "Internal Server Error");
            }
        }
        async Task<bool> DeleteOldGroups(ConditionRule conditionRuleEntity)
        {
            IGenericRepository<ConditionGroup> conditionGroupRepo = _unitOfWork.ConditionGroupRepository;
            IGenericRepository<ProductCondition> productConditionRepo = _unitOfWork.ProductConditionRepository;
            IGenericRepository<OrderCondition> orderConditionRepo = _unitOfWork.OrderConditionRepository;
            IGenericRepository<ProductConditionMapping> prodCondMapRepo = _unitOfWork.ProductConditionMappingRepository;


            // Delete old groups and old conditions
            List<ConditionGroup> oldGroups = (await conditionGroupRepo.Get(pageIndex: 0, pageSize: 0,
                filter: o => o.ConditionRuleId.Equals(conditionRuleEntity.ConditionRuleId), includeProperties: "ProductCondition")).ToList();
            if (oldGroups.Count > 0)
            {
                foreach (var group in oldGroups)
                {
                    var productConditions = group.ProductCondition.ToList();
                    foreach (var prodCond in productConditions)
                    {
                        prodCondMapRepo.Delete(id: Guid.Empty, filter: o => o.ProductConditionId.Equals(prodCond.ProductConditionId));
                    }

                    productConditionRepo.Delete(id: Guid.Empty, filter: o => o.ConditionGroupId.Equals(group.ConditionGroupId));
                    orderConditionRepo.Delete(id: Guid.Empty, filter: o => o.ConditionGroupId.Equals(group.ConditionGroupId));
                    conditionGroupRepo.Delete(id: group.ConditionGroupId);
                }
                await _unitOfWork.SaveAsync();
            }
            return true;
        }
        void InsertConditionGroup(List<ConditionGroupDto> conditionGroups, ConditionRule conditionRuleEntity)
        {
            IGenericRepository<ConditionGroup> conditionGroupRepo = _unitOfWork.ConditionGroupRepository;
            IGenericRepository<ProductCondition> productConditionRepo = _unitOfWork.ProductConditionRepository;
            IGenericRepository<OrderCondition> orderConditionRepo = _unitOfWork.OrderConditionRepository;
            IGenericRepository<MembershipCondition> membershipConditionRepo = _unitOfWork.MembershipConditionRepository;

            //Insert new condition groups
            foreach (var group in conditionGroups)
            {
                ConditionGroup conditionGroupEntity = new ConditionGroup
                {
                    ConditionGroupId = Guid.NewGuid(),
                    GroupNo = group.GroupNo,
                    ConditionRuleId = conditionRuleEntity.ConditionRuleId,
                    NextOperator = group.NextOperator,
                    Summary = "",
                    InsDate = DateTime.Now,
                    UpdDate = DateTime.Now,
                };
                conditionGroupEntity.Summary = CreateSummary(group);
                conditionGroupRepo.Add(conditionGroupEntity);
                group.ConditionGroupId = conditionGroupEntity.ConditionGroupId;
                // Create product condition
                if (group.ProductCondition != null && group.ProductCondition.Count > 0)
                {
                    IGenericRepository<ProductConditionMapping> mappRepo = _unitOfWork.ProductConditionMappingRepository;
                    foreach (var productCondition in group.ProductCondition)
                    {
                        var productConditionEntity = _mapper.Map<ProductCondition>(productCondition);
                        productConditionEntity.ConditionGroupId = conditionGroupEntity.ConditionGroupId;
                        productConditionEntity.ProductConditionId = Guid.NewGuid();
                        productConditionEntity.DelFlg = false;
                        productConditionEntity.UpdDate = DateTime.Now;
                        productConditionEntity.InsDate = DateTime.Now;
                        productConditionRepo.Add(productConditionEntity);
                        productCondition.ProductConditionId = productConditionEntity.ProductConditionId;
                        var products = productCondition.ListProduct;
                        foreach (var product in products)
                        {
                            var mapp = new ProductConditionMapping()
                            {
                                Id = Guid.NewGuid(),
                                ProductConditionId = productConditionEntity.ProductConditionId,
                                ProductId = product,
                                UpdTime = DateTime.Now,
                                InsDate = DateTime.Now,
                            };
                            mappRepo.Add(mapp);
                        }

                    }
                }
                // Create order condition
                if (group.OrderCondition != null && group.OrderCondition.Count > 0)
                {
                    foreach (var orderCondition in group.OrderCondition)
                    {
                        var orderConditionEntity = _mapper.Map<OrderCondition>(orderCondition);
                        orderConditionEntity.ConditionGroupId = conditionGroupEntity.ConditionGroupId;
                        orderConditionEntity.OrderConditionId = Guid.NewGuid();
                        orderConditionEntity.DelFlg = false;
                        orderConditionEntity.UpdDate = DateTime.Now;
                        orderConditionEntity.InsDate = DateTime.Now;
                        orderConditionRepo.Add(orderConditionEntity);
                        orderCondition.OrderConditionId = orderConditionEntity.OrderConditionId;
                    }
                }
            }

        }
        #endregion
        #region check voucher
        public async Task<OrderResponseModel> HandlePromotion(OrderResponseModel orderResponse)
        {
            try
            {
                var listPublicHoliday = await _holidayService.GetHolidays();
                _timeframeHandle.SetHolidays(listPublicHoliday);
                foreach (Promotion promotion in orderResponse.Promotions)
                {
                    //Check promotion is active
                    if (!promotion.Status.Equals(AppConstant.EnvVar.PromotionStatus.PUBLISH)) throw new ErrorObj(code: 400, message: AppConstant.ErrMessage.InActive_Promotion, description: AppConstant.ErrMessage.InActive_Promotion);
                    //Check promotion is time 
                    if (promotion.StartDate >= orderResponse.OrderDetail.BookingDate)
                    {
                        throw new ErrorObj(code: 400, message: AppConstant.ErrMessage.Invalid_Time, description: AppConstant.ErrMessage.Invalid_Early);
                    }
                    //Check promotion is expired
                    if (promotion.EndDate <= orderResponse.OrderDetail.BookingDate)
                    {
                        throw new ErrorObj(code: 400, message: AppConstant.ErrMessage.Expire_Promotion, description: AppConstant.ErrMessage.Expire_Promotion);
                    }
                }
                _applyPromotionHandler.Handle(orderResponse);

            }
            catch (ErrorObj e)
            {
                throw new ErrorObj(code: 400, message: e.Message);
            }
            catch (Exception ex)
            {
                throw new ErrorObj(code: 500, message: ex.Message);
            }
            return orderResponse;
        }
        #endregion
        #region update promotion
        public async Task<PromotionDto> UpdatePromotion(PromotionDto dto)
        {
            try
            {
                dto.UpdDate = DateTime.Now;
                if (dto.EndDate == null)
                {
                    IPromotionRepository promotionRepo = new PromotionRepositoryImp();
                    await promotionRepo.SetUnlimitedDate(_mapper.Map<Promotion>(dto));
                }
                var entity = _mapper.Map<Promotion>(dto);
                _repository.Update(entity);
                await _unitOfWork.SaveAsync();
                return _mapper.Map<PromotionDto>(entity);
            }
            catch (Exception ex)
            {
                throw new ErrorObj(code: 500, message: ex.Message);
            }


        }
        #endregion
        #region create summary for condition group
        private List<Object> ConvertConditionList(ConditionGroupDto group)
        {
            var totalCondition = 0;
            var productCond = false;
            var orderCond = false;

            if (group.ProductCondition != null && group.ProductCondition.Count > 0)
            {
                totalCondition += group.ProductCondition.Count;
                productCond = true;
            }
            if (group.OrderCondition != null && group.OrderCondition.Count > 0)
            {
                totalCondition += group.OrderCondition.Count;
                orderCond = true;
            }
            Object[] conditions = new Object[totalCondition];
            if (productCond)
            {
                foreach (var productCondition in group.ProductCondition)
                {
                    conditions[productCondition.IndexGroup] = productCondition;
                }
            }
            if (orderCond)
            {
                foreach (var orderCondition in group.OrderCondition)
                {
                    conditions[orderCondition.IndexGroup] = orderCondition;
                }
            }
            return conditions.ToList();
        }
        private string CreateSummary(ConditionGroupDto group)
        {
            var result = "";
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            var conditions = ConvertConditionList(group);
            for (int i = 0; i < conditions.Count; i++)
            {
                var condition = conditions[i];
                if (condition.GetType() == typeof(ProductConditionDto))
                {
                    var value = (ProductConditionDto)condition;
                    var productResult = "";
                    if (value.ProductConditionType.Equals("0"))
                    {
                        if (result.Equals(""))
                        {
                            productResult = "- Include ";
                        }
                        else
                        {
                            productResult = "include ";
                        }
                    }
                    else
                    {
                        if (result == "")
                        {
                            productResult = "- Exclude ";
                        }
                        else
                        {
                            productResult = "exclude ";
                        }
                    }

                    if (value.ProductConditionType.Equals("0"))
                    {
                        switch (value.QuantityOperator)
                        {
                            case "1":
                                {
                                    productResult += "more than ";
                                    break;
                                }
                            case "2":
                                {
                                    productResult += "more than or equal ";
                                    break;
                                }
                            case "3":
                                {
                                    productResult += "less than ";
                                    break;
                                }
                            case "4":
                                {
                                    productResult += "less than or equal ";
                                    break;
                                }
                        }
                    }
                    if (value.ProductConditionType.Equals("0"))
                    {
                        productResult += value.ProductQuantity + " ";
                    }
                    //productResult += value.ProductName;
                    if (i < conditions.Count - 1)
                    {
                        if (value.NextOperator.Equals("1"))
                        {
                            productResult += " or ";
                        }
                        else
                        {
                            productResult += " and ";
                        }
                    }

                    result += productResult;
                }
                if (condition.GetType() == typeof(OrderConditionDto))
                {
                    var value = (OrderConditionDto)condition;
                    var orderResult = "order has ";
                    if (result.Equals(""))
                    {
                        orderResult = "- Order has ";
                    }
                    switch (value.QuantityOperator)
                    {
                        case "1":
                            {
                                orderResult += "more than ";
                                break;
                            }
                        case "2":
                            {
                                orderResult += "more than or equal ";
                                break;
                            }
                        case "3":
                            {
                                orderResult += "less than ";
                                break;
                            }
                        case "4":
                            {
                                orderResult += "less than or equal ";
                                break;
                            }
                        case "5":
                            {
                                orderResult += "equal ";
                                break;
                            }
                    }
                    orderResult += value.Quantity + " item(s) and total ";
                    switch (value.AmountOperator)
                    {
                        case "1":
                            {
                                orderResult += "more than ";
                                break;
                            }
                        case "2":
                            {
                                orderResult += "more than or equal ";
                                break;
                            }
                        case "3":
                            {
                                orderResult += "less than ";
                                break;
                            }
                        case "4":
                            {
                                orderResult += "less than or equal ";
                                break;
                            }
                        case "5":
                            {
                                orderResult += "equal ";
                                break;
                            }
                    }
                    orderResult += double.Parse(value.Amount.ToString()).ToString("#,###", cul.NumberFormat) + " VNĐ";
                    if (i < conditions.Count - 1)
                    {
                        if (value.NextOperator.Equals("1"))
                        {
                            orderResult += " or ";
                        }
                        else
                        {
                            orderResult += " and ";
                        }
                    }

                    result += orderResult;
                }
                if (condition.GetType() == typeof(MembershipConditionDto))
                {
                    var value = (MembershipConditionDto)condition;
                    var membershipResult = "membership level are:  ";
                    if (result.Equals(""))
                    {
                        membershipResult = "- Membership level are:  ";
                    }
                    var list = "";
                    var levels = value.MembershipLevel.Split("|");
                    foreach (var level in levels)
                    {
                        if (list.Equals(""))
                        {
                            list += level;
                        }
                        else
                        {
                            list += ", " + level;
                        }
                    }
                    membershipResult += list;
                    if (i < conditions.Count - 1)
                    {
                        if (value.NextOperator.Equals("1"))
                        {
                            membershipResult += " or ";
                        }
                        else
                        {
                            membershipResult += " and ";
                        }
                    }

                    result += membershipResult;
                }
            }


            return result;
        }
        #endregion
        #region create summary for action
        private string CreateSummaryAction(Infrastructure.Models.Action entity)
        {
            var result = "";
            var actionType = entity.ActionType;
            var discountType = entity.DiscountType;
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            switch (actionType)
            {
                case "1":
                    {

                        switch (discountType)
                        {
                            case "1":
                                {
                                    result += "Discount ";
                                    result += double.Parse(entity.DiscountAmount.ToString()).ToString("#,###", cul.NumberFormat) + " VNĐ";
                                    if (entity.MinPriceAfter > 0)
                                    {
                                        result +=
                                          ", minimum residual price " +
                                           double.Parse(entity.MinPriceAfter.ToString()).ToString("#,###", cul.NumberFormat) + " VNĐ";


                                    }
                                    result += " for product";
                                    break;
                                }
                            case "2":
                                {
                                    result += "Discount ";
                                    result += entity.DiscountPercentage + "%";
                                    if (entity.MaxAmount > 0)
                                    {
                                        result += ", maximum ";
                                        result +=
                                            double.Parse(entity.MaxAmount.ToString()).ToString("#,###", cul.NumberFormat) + " VNĐ";

                                    }
                                    result += " for product";
                                    break;
                                }
                            case "3":
                                {
                                    result += "Free ";
                                    result += entity.DiscountQuantity + " unit(s) ";
                                    result += "of product";
                                    break;
                                }

                            case "5":
                                {
                                    result += "Fixed ";
                                    result +=
                                        double.Parse(entity.FixedPrice.ToString()).ToString("#,###", cul.NumberFormat) + " VNĐ";

                                    result += " for product";
                                    break;
                                }
                            case "6":
                                {
                                    result += "Buy from the ";
                                    result += ToOrdinal((long)entity.OrderLadderProduct);

                                    result += " product at the price of ";
                                    result +=
                                        double.Parse(entity.LadderPrice.ToString()).ToString("#,###", cul.NumberFormat) + " VNĐ";
                                    break;
                                }
                            case "7":
                                {
                                    result += "Buy ";
                                    result += entity.BundleQuantity + " product(s) for ";
                                    result +=
                                        double.Parse(entity.BundlePrice.ToString()).ToString("#,###", cul.NumberFormat) + " VNĐ";
                                    break;
                                }
                        }
                        break;
                    }
                case "2":
                    {

                        switch (discountType)
                        {
                            case "1":
                                {
                                    result += "Discount ";
                                    result +=
                                         double.Parse(entity.DiscountAmount.ToString()).ToString("#,###", cul.NumberFormat) + " VNĐ";

                                    if (entity.MinPriceAfter > 0)
                                    {
                                        result +=
                                          ", minimum residual price " +
                                           double.Parse(entity.MinPriceAfter.ToString()).ToString("#,###", cul.NumberFormat) + " VNĐ";


                                    }

                                    result += " for order";
                                    break;
                                }
                            case "2":
                                {
                                    result += "Discount ";
                                    result += entity.DiscountPercentage + "%";
                                    if (entity.MaxAmount > 0)
                                    {
                                        result += ", maximum ";
                                        result +=
                                             double.Parse(entity.MaxAmount.ToString()).ToString("#,###", cul.NumberFormat) + " VNĐ";

                                    }
                                    result += " for order";
                                    break;
                                }
                            case "4":
                                {
                                    result += "Discount ";
                                    if (entity.DiscountAmount != 0)
                                    {
                                        result +=
                                             double.Parse(entity.DiscountAmount.ToString()).ToString("#,###", cul.NumberFormat) + " VNĐ";

                                    }
                                    else
                                    {
                                        result += entity.DiscountPercentage + "% ";
                                        if (entity.MaxAmount > 0)
                                        {
                                            result += ", maximum ";
                                            result +=
                                                double.Parse(entity.MaxAmount.ToString()).ToString("#,###", cul.NumberFormat) + " VNĐ";

                                        }
                                    }
                                    result += " for shipping of order";

                                    break;
                                }
                        }
                        break;
                    }
            }
            return result;
        }

        /*   private string CreateSummarypostAction(PostAction entity)
           {
               var result = "";
               var actionType = entity.ActionType;
               var discountType = entity.DiscountType;
               switch (actionType)
               {
                   case "3":
                       {

                           switch (discountType)
                           {
                               case "8":
                                   {
                                       result += "Gift ";
                                       result += entity.GiftQuantity;
                                       result += " " + entity.GiftName + "(s)";
                                       break;
                                   }
                               case "9":
                                   {
                                       result += "Gift a voucher code: ";
                                       result += entity.GiftVoucherCode;
                                       break;
                                   }
                           }
                           return result;
                       }
                   case "4":
                       {
                           result += "Bonus point: ";
                           result += entity.BonusPoint + " point(s)";
                           return result;
                       }
               }
               return result;
           }
   */
        private string ToOrdinal(long number)
        {
            if (number < 0) return number.ToString();
            long rem = number % 100;
            if (rem >= 11 && rem <= 13) return number + "th";

            switch (number % 10)
            {
                case 1:
                    return number + "st";
                case 2:
                    return number + "nd";
                case 3:
                    return number + "rd";
                default:
                    return number + "th";
            }
        }
        #endregion
        #region statistic
        public async Task<PromotionStatusDto> CountPromotionStatus(Guid brandId)
        {
            if (brandId.Equals(Guid.Empty))
            {
                throw new ErrorObj(code: 400, message: AppConstant.StatisticMessage.BRAND_ID_INVALID, description: "Internal Server Error");
            }
            try
            {
                var result = new PromotionStatusDto()
                {
                    Total = await _repository.CountAsync(filter: o => o.BrandId.Equals(brandId)
                                && !o.DelFlg),
                    Draft = await _repository.CountAsync(filter: o => o.BrandId.Equals(brandId)
                                && o.Status.Equals(AppConstant.EnvVar.PromotionStatus.DRAFT)
                                && !o.DelFlg),
                    Publish = await _repository.CountAsync(filter: o => o.BrandId.Equals(brandId)
                                && o.Status.Equals(AppConstant.EnvVar.PromotionStatus.PUBLISH)
                                && !o.DelFlg),
                    Unpublish = await _repository.CountAsync(filter: o => o.BrandId.Equals(brandId)
                                && o.Status.Equals(AppConstant.EnvVar.PromotionStatus.UNPUBLISH)
                                && !o.DelFlg),
                    Expired = await _repository.CountAsync(filter: o => o.BrandId.Equals(brandId)
                                && o.Status.Equals(AppConstant.EnvVar.PromotionStatus.EXPIRED)
                                && !o.DelFlg)
                };

                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw new ErrorObj(code: 500, message: AppConstant.StatisticMessage.PROMO_COUNT_ERR, description: "Internal Server Error");
            }

        }

        public async Task<DistributionStat> DistributionStatistic(Guid promotionId, Guid brandId)
        {

            try
            {
                IGenericRepository<PromotionStoreMapping> storeMappRepo = _unitOfWork.PromotionStoreMappingRepository;
                IGenericRepository<PromotionChannelMapping> channelMappRepo = _unitOfWork.VoucherChannelRepository;
                IGenericRepository<Store> storeRepo = _unitOfWork.StoreRepository;
                IGenericRepository<Channel> channelRepo = _unitOfWork.ChannelRepository;
                IGenericRepository<Voucher> voucherRepo = _unitOfWork.VoucherRepository;

                var voucherGroup = (await _repository.GetFirst(filter: o => o.PromotionId.Equals(promotionId)
                && o.BrandId.Equals(brandId),
                    includeProperties: "VoucherGroup"))
                    .VoucherGroup;
                var voucherGroupId = Guid.Empty;
                if (voucherGroup != null)
                {
                    voucherGroupId = voucherGroup.VoucherGroupId;
                }

                var result = new DistributionStat()
                {
                    ChannelStat = new List<GroupChannel>(),
                    StoreStat = new List<GroupStore>(),
                };

                var storeMapp = (await storeRepo.Get(filter: o => !o.DelFlg && o.BrandId.Equals(brandId))).ToList();
                var storeStatList = new List<StoreVoucherStat>();
                foreach (var store in storeMapp)
                {
                    var storeStat = new StoreVoucherStat()
                    {
                        StoreId = store.StoreId,
                        StoreName = store.StoreName,
                        RedempVoucherCount = await voucherRepo.CountAsync(filter: o => o.StoreId.Equals(store.StoreId) && o.IsRedemped
                        && o.VoucherGroupId.Equals(voucherGroupId)),
                        GroupNo = (int)store.Group,
                    };
                    storeStatList.Add(storeStat);
                }

                var storeGroups = storeStatList.GroupBy(o => o.GroupNo).Select(o => o.Distinct()).ToList();
                foreach (var group in storeGroups)
                {
                    var listStore = group.ToList();
                    var groupStore = new GroupStore()
                    {
                        GroupNo = listStore.First().GroupNo,
                        Stores = listStore
                    };
                    result.StoreStat.Add(groupStore);
                }


                var channelMapp = (await channelRepo.Get(filter: o => !o.DelFlg && o.BrandId.Equals(brandId))).ToList();
                var channelStatList = new List<ChannelVoucherStat>();
                foreach (var channel in channelMapp)
                {
                    var channelStat = new ChannelVoucherStat()
                    {
                        ChannelId = channel.ChannelId,
                        ChannelName = channel.ChannelName,
                        RedempVoucherCount = await voucherRepo.CountAsync(filter: o => o.StoreId.Equals(channel.ChannelId) && o.IsRedemped
                        && o.VoucherGroupId.Equals(voucherGroupId)),
                        GroupNo = (int)channel.Group,
                    };
                    channelStatList.Add(channelStat);
                }

                var channelGroups = channelStatList.GroupBy(o => o.GroupNo).Select(o => o.Distinct()).ToList();
                foreach (var group in channelGroups)
                {
                    var listChannel = group.ToList();
                    var groupChannel = new GroupChannel()
                    {
                        GroupNo = listChannel.First().GroupNo,
                        Channels = listChannel
                    };
                    result.ChannelStat.Add(groupChannel);
                }


                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw new ErrorObj(code: 500, message: "Oops, something went wrong. Try again", description: "Internal Server Error");
            }

        }
        #endregion
    }
}
