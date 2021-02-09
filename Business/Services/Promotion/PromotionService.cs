
using ApplicationCore.Chain;
using ApplicationCore.Request;
using ApplicationCore.Utils;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using Microsoft.EntityFrameworkCore;
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
        private readonly IApplyPromotionHandler _applyPromotionHandler;
        public PromotionService(IUnitOfWork unitOfWork, IMapper mapper, IApplyPromotionHandler promotionHandle) : base(unitOfWork, mapper)
        {
            _applyPromotionHandler = promotionHandle;
        }

        protected override IGenericRepository<Promotion> _repository => _unitOfWork.PromotionRepository;



        public async Task<PromotionTierParam> CreatePromotionTier(PromotionTierParam param)
        {
            try
            {

                IGenericRepository<ConditionRule> conditionRuleRepo = _unitOfWork.ConditionRuleRepository;
                IGenericRepository<ConditionGroup> conditionGroupRepo = _unitOfWork.ConditionGroupRepository;
                IGenericRepository<ProductCondition> productConditionRepo = _unitOfWork.ProductConditionRepository;
                IGenericRepository<OrderCondition> orderConditionRepo = _unitOfWork.OrderConditionRepository;
                IGenericRepository<MembershipCondition> membershipConditionRepo = _unitOfWork.MembershipConditionRepository;
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
                    List<ConditionGroup> oldGroups = (await conditionGroupRepo.Get(pageIndex: 0, pageSize: 0, filter: o => o.ConditionRuleId.Equals(conditionRuleEntity.ConditionRuleId))).ToList();
                    if (oldGroups.Count > 0)
                    {
                        foreach (var group in oldGroups)
                        {
                            productConditionRepo.Delete(id: Guid.Empty, filter: o => o.ConditionGroupId.Equals(group.ConditionGroupId));
                            orderConditionRepo.Delete(id: Guid.Empty, filter: o => o.ConditionGroupId.Equals(group.ConditionGroupId));
                            membershipConditionRepo.Delete(id: Guid.Empty, filter: o => o.ConditionGroupId.Equals(group.ConditionGroupId));
                            conditionGroupRepo.Delete(id: group.ConditionGroupId);
                        }
                        await _unitOfWork.SaveAsync();
                    }


                }
                // Create condition group
                foreach (var group in param.ConditionGroups)
                {
                    ConditionGroup conditionGroupEntity = new ConditionGroup
                    {
                        ConditionGroupId = Guid.NewGuid(),
                        GroupNo = group.GroupNo,
                        ConditionRuleId = conditionRuleEntity.ConditionRuleId,
                        NextOperator = group.NextOperator,
                        InsDate = DateTime.Now,
                        UpdDate = DateTime.Now
                    };
                    conditionGroupRepo.Add(conditionGroupEntity);
                    group.ConditionGroupId = conditionGroupEntity.ConditionGroupId;
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
                            productCondition.ProductConditionId = productConditionEntity.ProductConditionId;

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
                            orderCondition.OrderConditionId = orderConditionEntity.OrderConditionId;

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
                            membershipCondition.MembershipConditionId = membershipConditionEntity.MembershipConditionId;

                        }
                    }

                }



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
                    param.Action = _mapper.Map<ActionRequestParam>(actionEntity);

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
                    param.MembershipAction = _mapper.Map<MembershipActionRequestParam>(membershipAction);
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

        public async Task<bool> DeletePromotionTier(DeleteTierRequestParam param)
        {
            try
            {
                // Delete action or membership action
                if (!param.ActionId.Equals(Guid.Empty))
                {
                    IGenericRepository<Infrastructure.Models.Action> actionRepo = _unitOfWork.ActionRepository;
                    actionRepo.Delete(param.ActionId);
                }
                else if (!param.MembershipActionId.Equals(Guid.Empty))
                {
                    IGenericRepository<MembershipAction> membershipActionRepo = _unitOfWork.MembershipActionRepository;
                    membershipActionRepo.Delete(param.MembershipActionId);
                }
                else
                {
                    throw new ErrorObj(code: 400, message: "Action or Membership action is not null", description: "Invalid param");
                }
                // Delete promotion tier
                IGenericRepository<PromotionTier> tierRepo = _unitOfWork.PromotionTierRepository;
                tierRepo.Delete(param.PromotionTierId);
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (ErrorObj e)
            {
                throw e;
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
                    includeProperties: "ConditionRule,ConditionRule.ConditionGroup,ConditionRule.ConditionGroup.MembershipCondition,ConditionRule.ConditionGroup.OrderCondition,ConditionRule.ConditionGroup.ProductCondition,MembershipAction,Action"))
                    .ToList();
                return tiers;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.GetType());
                Debug.WriteLine(e.StackTrace);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.", description: "Internal Server Error");
            }

            //return result;

        }



        #region check voucher
        public async Task<OrderResponseModel> HandlePromotion(OrderResponseModel orderResponse)
        {
            try
            {
                var now = Common.GetCurrentDatetime();
                foreach (Promotion promotion in orderResponse.Promotions)
                {
                    //Check promotion is active
                    if (!promotion.IsActive) throw new ErrorObj(code: 400, message: AppConstant.ErrMessage.InActive_Promotion, description: AppConstant.ErrMessage.InActive_Promotion);
                    //Check promotion is time
                    if (promotion.StartDate > now)
                    {
                        throw new ErrorObj(code: 400, message: AppConstant.ErrMessage.Invalid_Time, description: AppConstant.ErrMessage.Invalid_Time);
                    }
                    //Check promotion is expired
                    if (promotion.EndDate < now)
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
                throw new ErrorObj(code: 500, message: ex.ToString());
            }
            return orderResponse;
        }
        #endregion
    }
}
