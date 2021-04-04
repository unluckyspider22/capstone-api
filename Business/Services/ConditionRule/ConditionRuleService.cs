using AutoMapper;
using Infrastructure.DTOs;
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
    public class ConditionRuleService : BaseService<ConditionRule, ConditionRuleDto>, IConditionRuleService
    {
        private readonly IActionService _actionService;
        private readonly IPostActionService _postActionService;
        public ConditionRuleService(IUnitOfWork unitOfWork, IMapper mapper, IActionService actionService, IPostActionService postActionService) : base(unitOfWork, mapper)
        {
            _actionService = actionService;
            _postActionService = postActionService;
        }

        protected override IGenericRepository<ConditionRule> _repository => _unitOfWork.ConditionRuleRepository;

        public async Task<bool> Delete(Guid conditionRuleId)
        {
            IGenericRepository<ConditionGroup> groupRepo = _unitOfWork.ConditionGroupRepository;
            IGenericRepository<ProductCondition> productRepo = _unitOfWork.ProductConditionRepository;
            IGenericRepository<ProductConditionMapping> mappRepo = _unitOfWork.ProductConditionMappingRepository;
            IGenericRepository<OrderCondition> orderRepo = _unitOfWork.OrderConditionRepository;
            IGenericRepository<PromotionTier> tierRepo = _unitOfWork.PromotionTierRepository;
            try
            {
                var conditionRule = await _repository.GetFirst(filter: o => o.ConditionRuleId.Equals(conditionRuleId),
                    includeProperties: "ConditionGroup," +
                    "ConditionGroup.ProductCondition," +
                    "ConditionGroup.OrderCondition," +
                    "PromotionTier," +
                    "PromotionTier.Action," +
                    "PromotionTier.PostAction");
                var groups = conditionRule.ConditionGroup.ToList();
                if (groups != null && groups.Count > 0)
                {
                    foreach (var group in groups)
                    {
                        var productConds = group.ProductCondition;
                        var orderConds = group.OrderCondition;
                        if (productConds != null && productConds.Count > 0)
                        {
                            foreach (var product in productConds)
                            {
                                mappRepo.Delete(id: Guid.Empty, filter: o => o.ProductConditionId.Equals(product.ProductConditionId));
                                productRepo.Delete(id: product.ProductConditionId);
                            }
                            await _unitOfWork.SaveAsync();
                        }

                        if (orderConds != null && orderConds.Count > 0)
                        {
                            foreach (var orderCond in orderConds)
                            {
                                orderRepo.Delete(id: orderCond.OrderConditionId);
                            }
                            await _unitOfWork.SaveAsync();
                        }
                        groupRepo.Delete(id: group.ConditionGroupId);
                    }
                }
                var promotionTier = conditionRule.PromotionTier;
                if (promotionTier.Action != null)
                {
                    await _actionService.Delete((Guid)promotionTier.ActionId);
                }
                else if (promotionTier.PostAction != null)
                {
                    await _postActionService.Delete((Guid)promotionTier.PostActionId);
                }
                tierRepo.Delete(id: promotionTier.PromotionTierId);
                _repository.Delete(id: conditionRuleId);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.InnerException);
                throw new ErrorObj(code: 500, message: e.Message, description: "Internal Server Error");
            }

        }

        public async Task<ConditionRuleDto> InsertConditionRule(ConditionRuleDto param)
        {
            try
            {
                // Insert condition rule
                var ruleEntity = _mapper.Map<ConditionRule>(param);
                ruleEntity.ConditionRuleId = Guid.NewGuid();
                ruleEntity.InsDate = DateTime.Now;
                ruleEntity.UpdDate = DateTime.Now;
                _repository.Add(ruleEntity);
                await _unitOfWork.SaveAsync();
                return _mapper.Map<ConditionRuleDto>(ruleEntity);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.InnerException);
                throw new ErrorObj(code: 500, message: e.Message, description: "Internal Server Error");
            }


        }

        public async Task<List<ConditionRuleResponse>> ReorderResult(List<ConditionRule> paramList)
        {
            List<ConditionRuleResponse> result = new List<ConditionRuleResponse>();
            IGenericRepository<ProductConditionMapping> mappRepo = _unitOfWork.ProductConditionMappingRepository;
            IGenericRepository<ProductCategory> cateRepo = _unitOfWork.ProductCategoryRepository;

            foreach (var rule in paramList)
            {
                ConditionRuleResponse conditionRuleResponse = new ConditionRuleResponse
                {
                    ConditionRuleId = rule.ConditionRuleId,
                    BrandId = rule.BrandId,
                    RuleName = rule.RuleName,
                    Description = rule.Description,
                    ConditionGroups = new List<ConditionGroupResponse>(),
                };
                //if (rule.PromotionTier.PromotionId != null)
                //{
                //    conditionRuleResponse.PromotionId = rule.PromotionTier.PromotionId;
                //    conditionRuleResponse.PromotionName = rule.PromotionTier.Promotion.PromotionName;
                //}

                ICollection<ConditionGroup> groupsParam = rule.ConditionGroup;
                foreach (var group in groupsParam)
                {
                    ConditionGroupResponse groupResponse = new ConditionGroupResponse
                    {
                        ConditionRuleId = rule.ConditionRuleId,
                        ConditionGroupId = group.ConditionGroupId,
                        GroupNo = group.GroupNo,
                        NextOperator = group.NextOperator,
                        Conditions = new List<object>(),
                    };
                    if (group.ProductCondition != null && group.ProductCondition.Count > 0)
                    {

                        var productConditions = group.ProductCondition;

                        foreach (var productCondition in productConditions)
                        {
                            var dto = _mapper.Map<ProductConditionTierDto>(productCondition);
                            dto.ListProduct = new List<ProductDto>();
                            var mapps = (await mappRepo.Get(filter: o => o.ProductConditionId.Equals(dto.ProductConditionId),
                                includeProperties: "Product")).ToList();
                            if (mapps != null && mapps.Count > 0)
                            {
                                foreach (var mapp in mapps)
                                {
                                    var product = mapp.Product;
                                    var cate = await cateRepo.GetFirst(filter: o => o.ProductCateId.Equals(product.ProductCateId)
                                    && !o.DelFlg);
                                    var productDto = new ProductDto()
                                    {
                                        CateId = cate != null ? cate.CateId : "",
                                        ProductCateId = cate != null ? cate.ProductCateId : Guid.Empty,
                                        Code = product.Code,
                                        Name = product.Name,
                                        ProductId = product.ProductId,
                                    };
                                    dto.ListProduct.Add(productDto);
                                }
                            }

                            groupResponse.Conditions.Add(dto);
                        }
                    }
                    if (group.OrderCondition != null && group.OrderCondition.Count > 0)
                    {
                        var orderConditions = group.OrderCondition;
                        foreach (var orderCondition in orderConditions)
                        {
                            groupResponse.Conditions.Add(_mapper.Map<OrderConditionDto>(orderCondition));
                        }
                    }
                    groupResponse.Conditions = groupResponse.Conditions.OrderBy(
                        o => (o.GetType() == typeof(ProductConditionTierDto)
                        ? ((ProductConditionTierDto)o).IndexGroup
                        : ((OrderConditionDto)o).IndexGroup)).ToList();
                    conditionRuleResponse.ConditionGroups.Add(groupResponse);
                }
                conditionRuleResponse.ConditionGroups = conditionRuleResponse.ConditionGroups.OrderBy(o => o.GroupNo).ToList();
                result.Add(conditionRuleResponse);
            }
            return result;
        }

        public async Task<ConditionRuleResponse> ReorderResult(ConditionRule param)
        {
            IGenericRepository<ProductConditionMapping> mappRepo = _unitOfWork.ProductConditionMappingRepository;
            IGenericRepository<ProductCategory> cateRepo = _unitOfWork.ProductCategoryRepository;
            ConditionRuleResponse conditionRuleResponse = new ConditionRuleResponse
            {
                ConditionRuleId = param.ConditionRuleId,
                BrandId = param.BrandId,
                RuleName = param.RuleName,
                Description = param.Description,
                ConditionGroups = new List<ConditionGroupResponse>(),
            };
            ICollection<ConditionGroup> groupsParam = param.ConditionGroup;
            foreach (var group in groupsParam)
            {
                ConditionGroupResponse groupResponse = new ConditionGroupResponse
                {
                    ConditionRuleId = param.ConditionRuleId,
                    ConditionGroupId = group.ConditionGroupId,
                    Summary = group.Summary,
                    GroupNo = group.GroupNo,
                    NextOperator = group.NextOperator,
                    Conditions = new List<object>(),
                };
                if (group.ProductCondition != null && group.ProductCondition.Count > 0)
                {

                    var productConditions = group.ProductCondition;
                    foreach (var productCondition in productConditions)
                    {

                        var dto = _mapper.Map<ProductConditionTierDto>(productCondition);
                        dto.ListProduct = new List<ProductDto>();

                        // Lấy các record trong bảng Product Condition Mapping
                        var mapps = (await mappRepo.Get(filter: o => o.ProductConditionId.Equals(dto.ProductConditionId),
                            includeProperties: "Product")).ToList();

                        // Nếu có record
                        if (mapps != null && mapps.Count > 0)
                        {
                            foreach (var mapp in mapps)
                            {
                                var product = mapp.Product;

                                // Lấy category của product
                                var cate = await cateRepo.GetFirst(filter: o => o.ProductCateId.Equals(product.ProductCateId)
                                && !o.DelFlg);
                                var productDto = new ProductDto()
                                {
                                    CateId = cate != null ? cate.CateId : "",
                                    ProductCateId = cate != null ? cate.ProductCateId : Guid.Empty,
                                    Code = product.Code,
                                    Name = product.Name,
                                    ProductId = product.ProductId,
                                };
                                dto.ListProduct.Add(productDto);
                            }
                        }
                        groupResponse.Conditions.Add(dto);
                    }
                }
                if (group.OrderCondition != null && group.OrderCondition.Count > 0)
                {
                    var orderConditions = group.OrderCondition;
                    foreach (var orderCondition in orderConditions)
                    {
                        var dto = _mapper.Map<OrderConditionDto>(orderCondition);
                        groupResponse.Conditions.Add(dto);
                    }
                }
                groupResponse.Conditions = groupResponse.Conditions.OrderBy(
                    o => (o.GetType() == typeof(ProductConditionTierDto)
                    ? ((ProductConditionTierDto)o).IndexGroup
                    : ((OrderConditionDto)o).IndexGroup)).ToList();
                conditionRuleResponse.ConditionGroups.Add(groupResponse);
                conditionRuleResponse.ConditionGroups = conditionRuleResponse.ConditionGroups.OrderBy(o => o.GroupNo).ToList();
            }

            return conditionRuleResponse;
        }
    }
}
