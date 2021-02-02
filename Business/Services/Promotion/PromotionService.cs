
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class PromotionService : BaseService<Promotion, PromotionDto>, IPromotionService
    {
        public PromotionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Promotion> _repository => _unitOfWork.PromotionRepository;

        public async Task<PromotionTierParam> CreatePromotionTier(PromotionTierParam param)
        {
            try
            {
                // Create condition rule
                IGenericRepository<ConditionRule> conditionRuleRepo = _unitOfWork.ConditionRuleRepository;
                var conditionRuleEntity = _mapper.Map<ConditionRule>(param.ConditionRule);
                conditionRuleEntity.ConditionRuleId = Guid.NewGuid();
                conditionRuleRepo.Add(conditionRuleEntity);
                //await _unitOfWork.SaveAsync();

                // Create condition group
                IGenericRepository<ConditionGroup> conditionGroupRepo = _unitOfWork.ConditionGroupRepository;
                IGenericRepository<ProductCondition> productConditionRepo = _unitOfWork.ProductConditionRepository;
                IGenericRepository<OrderCondition> orderConditionRepo = _unitOfWork.OrderConditionRepository;
                IGenericRepository<MembershipCondition> membershipConditionRepo = _unitOfWork.MembershipConditionRepository;
                foreach (var group in param.ConditionGroups)
                {
                    ConditionGroup conditionGroupEntity = new ConditionGroup
                    {
                        ConditionGroupId = Guid.NewGuid(),
                        GroupNo = group.GroupNo,
                        ConditionRuleId = conditionRuleEntity.ConditionRuleId,
                        InsDate = DateTime.Now,
                        UpdDate = DateTime.Now
                    };
                    conditionGroupRepo.Add(conditionGroupEntity);
                    //await _unitOfWork.SaveAsync();

                    // Create product condition
                    if (group.ProductCondition.Count > 0)
                    {
                        foreach (var productCondition in group.ProductCondition)
                        {
                            var productConditionEntity = _mapper.Map<ProductCondition>(productCondition);
                            productConditionEntity.ConditionGroupId = conditionGroupEntity.ConditionGroupId;
                            productConditionEntity.ProductConditionId = Guid.NewGuid();
                            productConditionEntity.DelFlg = false;
                            productConditionEntity.UpdDate = DateTime.Now;
                            productConditionEntity.InsDate = DateTime.Now;
                            productConditionRepo.Add(productConditionEntity);

                        }
                    }

                    // Create order condition
                    if (group.OrderCondition.Count > 0)
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

                        }
                    }
                   
                    // Create membership condition
                    if (group.MembershipCondition.Count > 0)
                    {
                        foreach (var membershipCondition in group.MembershipCondition)
                        {
                            var membershipConditionEntity = _mapper.Map<MembershipCondition>(membershipCondition);
                            membershipConditionEntity.ConditionGroupId = conditionGroupEntity.ConditionGroupId;
                            membershipConditionEntity.MembershipConditionId = Guid.NewGuid();
                            membershipConditionEntity.DelFlg = false;
                            membershipConditionEntity.UpdDate = DateTime.Now;
                            membershipConditionEntity.InsDate = DateTime.Now;
                            membershipConditionRepo.Add(membershipConditionEntity);

                        }
                    }
                   
                }
                //await _unitOfWork.SaveAsync();

                // Create promotion tier
                IGenericRepository<PromotionTier> promotionTierRepo = _unitOfWork.PromotionTierRepository;
                PromotionTier promotionTier = new PromotionTier
                {
                    PromotionTierId = Guid.NewGuid(),
                    PromotionId = param.PromotionId,
                    ConditionRuleId = conditionRuleEntity.ConditionRuleId,

                };

                // Create action
                if (param.Action.ActionType != null)
                {
                    IGenericRepository<Infrastructure.Models.Action> actionRepo = _unitOfWork.ActionRepository;
                    var actionEntity = _mapper.Map<Infrastructure.Models.Action>(param.Action);
                    actionEntity.ActionId = Guid.NewGuid();
                    actionEntity.PromotionTierId = promotionTier.PromotionTierId;
                    promotionTier.ActionId = actionEntity.ActionId;
                    promotionTierRepo.Add(promotionTier);
                    actionRepo.Add(actionEntity);

                }
                else
                if (param.MembershipAction.ActionType != null)
                {
                    // Create membership action
                    IGenericRepository<MembershipAction> membershipActionRepo = _unitOfWork.MembershipActionRepository;
                    var membershipAction = _mapper.Map<MembershipAction>(param.MembershipAction);
                    membershipAction.MembershipActionId = Guid.NewGuid();
                    membershipAction.PromotionTierId = promotionTier.PromotionTierId;
                    promotionTier.MembershipActionId = membershipAction.MembershipActionId;
                    promotionTierRepo.Add(promotionTier);
                    membershipActionRepo.Add(membershipAction);
                }
                else
                {
                    throw new Exception();
                }
                await _unitOfWork.SaveAsync();



                return param;


            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.", description: "Internal Server Error");
            }
        }

        public async Task<List<PromotionTier>> GetPromotionTierDetail(Guid promotionId)
        {

            IGenericRepository<PromotionTier> _tierRepo = _unitOfWork.PromotionTierRepository;
            try
            {
                // Lấy danh sách promotion tier
                Expression<Func<PromotionTier, bool>> filter = el => el.PromotionId.Equals(promotionId);
                var tiers = (
                    await _tierRepo.Get(0, 0, filter: filter,
                    includeProperties: "ConditionRule,MembershipAction,Action"))
                    .ToList();
                return tiers;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.", description: "Internal Server Error");
            }

            //return result;

        }


    }
}
