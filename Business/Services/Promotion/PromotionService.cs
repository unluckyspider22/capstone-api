
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

        public async Task<PromotionTierParam> CreatePromotionTier(Guid promotionId, PromotionTierParam param)
        {
            try
            {
                Debug.WriteLine("Rule name: " + param.Condition.conditionRule.RuleName);
                Debug.WriteLine("Action type: " + param.Action.ActionType);
                Debug.WriteLine("Membership Action type: " + param.MembershipAction.ActionType);
                // Insert condition rule
                ConditionParamDto condition = await InsertConditions(param.Condition);

                // Insert promotion tier
                PromotionTierDto promotionTier;

                // Insert action or membership condition
                ActionDto action = new ActionDto();
                MembershipActionDto membershipAction = new MembershipActionDto();
                if (param.Action.ActionType != null)
                {
                    param.Action.ActionId = Guid.NewGuid();
                    promotionTier = await InsertPromotionTier(condition.conditionRule.ConditionRuleId, param.Action.ActionId, Guid.Empty, promotionId);
                    param.Action.PromotionTierId = promotionTier.PromotionTierId;
                    
                    action = await InsertAction(param.Action);
                }
                else
                if (param.MembershipAction.ActionType != null)
                {
                    param.MembershipAction.MembershipActionId = Guid.NewGuid();
                    promotionTier = await InsertPromotionTier(condition.conditionRule.ConditionRuleId, Guid.Empty, param.MembershipAction.MembershipActionId, promotionId);
                    param.MembershipAction.PromotionTierId = promotionTier.PromotionTierId;
                    membershipAction = await InsertMembershipAction(param.MembershipAction);
                }
                PromotionTierParam result = new PromotionTierParam
                {
                    Condition = condition,
                    Action = action,
                    MembershipAction = membershipAction,
                };
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.", description: "Internal Server Error");
            }
        }

        public async Task<List<PromotionTier>> GetPromotionTierDetail(Guid promotionId)
        {
            //List<PromotionTierResponse> result = new List<PromotionTierResponse>();

            IGenericRepository<PromotionTier> _tierRepo = _unitOfWork.PromotionTierRepository;
            try
            {
                // Lấy danh sách promotion tier
                Expression<Func<PromotionTier, bool>> filter = el => el.PromotionId.Equals(promotionId);
                var tiers = (
                    await _tierRepo.Get(0, 0, filter: filter,
                    includeProperties: "ConditionRule,ConditionRule.ProductCondition,ConditionRule.OrderCondition,ConditionRule.MembershipCondition,MembershipAction,Action"))
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

        #region handle insert conditions
        private async Task<ConditionParamDto> InsertConditions(ConditionParamDto param)
        {
            ConditionParamDto response = new ConditionParamDto
            {
                // Insert condition rule
                conditionRule = await InsertConditionRule(param),
                // Insert list product condition
                productConditions = await InsertProductCondition(param),
                // Insert list order condition
                orderConditions = await InsertOrderCondition(param),
                // Insert list membership condition
                membershipConditions = await InsertMembershipCondition(param),
            };
            return response;
        }

        private async Task<ConditionRuleDto> InsertConditionRule(ConditionParamDto param)
        {
            IGenericRepository<ConditionRule> _conditionRuleRepo = _unitOfWork.ConditionRuleRepository;
            param.conditionRule.ConditionRuleId = Guid.NewGuid();
            var conditionRuleEntity = _mapper.Map<ConditionRule>(param.conditionRule);
            _conditionRuleRepo.Add(conditionRuleEntity);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<ConditionRuleDto>(conditionRuleEntity);
        }
        private async Task<List<ProductConditionDto>> InsertProductCondition(ConditionParamDto param)
        {
            List<ProductConditionDto> result = new List<ProductConditionDto>();
            IGenericRepository<ProductCondition> _productConditionRepo = _unitOfWork.ProductConditionRepository;
            foreach (var condition in param.productConditions)
            {
                condition.ProductConditionId = Guid.NewGuid();
                condition.ConditionRuleId = param.conditionRule.ConditionRuleId;
                var productConditionEntity = _mapper.Map<ProductCondition>(condition);
                _productConditionRepo.Add(productConditionEntity);
                await _unitOfWork.SaveAsync();
                result.Add(_mapper.Map<ProductConditionDto>(productConditionEntity));

            }

            return result;
        }
        private async Task<List<OrderConditionDto>> InsertOrderCondition(ConditionParamDto param)
        {
            List<OrderConditionDto> result = new List<OrderConditionDto>();
            IGenericRepository<OrderCondition> _orderConditionRepo = _unitOfWork.OrderConditionRepository;

            foreach (var condition in param.orderConditions)
            {
                condition.OrderConditionId = Guid.NewGuid();
                condition.ConditionRuleId = param.conditionRule.ConditionRuleId;
                var orderConditionEntity = _mapper.Map<OrderCondition>(condition);
                _orderConditionRepo.Add(orderConditionEntity);
                await _unitOfWork.SaveAsync();
                result.Add(_mapper.Map<OrderConditionDto>(orderConditionEntity));

            }
            return result;
        }
        private async Task<List<MembershipConditionDto>> InsertMembershipCondition(ConditionParamDto param)
        {
            List<MembershipConditionDto> result = new List<MembershipConditionDto>();
            IGenericRepository<MembershipCondition> _membershipRuleRepo = _unitOfWork.MembershipConditionRepository;
            foreach (var condition in param.membershipConditions)
            {
                condition.MembershipConditionId = Guid.NewGuid();
                condition.ConditionRuleId = param.conditionRule.ConditionRuleId;
                var membershipConditionEntity = _mapper.Map<MembershipCondition>(condition);
                _membershipRuleRepo.Add(membershipConditionEntity);
                await _unitOfWork.SaveAsync();
                result.Add(_mapper.Map<MembershipConditionDto>(membershipConditionEntity));

            }
            return result;
        }
        #endregion
        #region handle insert promotion tier
        private async Task<PromotionTierDto> InsertPromotionTier(Guid conditionRuleId, Guid actionId, Guid membershipActionId, Guid promotionId)
        {
            IGenericRepository<PromotionTier> _tierRepo = _unitOfWork.PromotionTierRepository;
            PromotionTier tierEntity = new PromotionTier
            {
                PromotionTierId = Guid.NewGuid(),
                ConditionRuleId = conditionRuleId,
                PromotionId = promotionId,
            };
            if (actionId != Guid.Empty)
            {
                tierEntity.ActionId = actionId;
            }
            if (membershipActionId != Guid.Empty)
            {
                tierEntity.MembershipActionId = membershipActionId;
            }
            _tierRepo.Add(tierEntity);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<PromotionTierDto>(tierEntity);
        }
        #endregion
        #region handle insert action
        private async Task<ActionDto> InsertAction(ActionDto param)
        {
            IGenericRepository<Infrastructure.Models.Action> _actionRepo = _unitOfWork.ActionRepository;

            var actionEntity = _mapper.Map<Infrastructure.Models.Action>(param);
            _actionRepo.Add(actionEntity);
            await _unitOfWork.SaveAsync();
            var response = _mapper.Map<ActionDto>(actionEntity);
            return response;
        }
        #endregion
        #region handle insert membership action
        private async Task<MembershipActionDto> InsertMembershipAction(MembershipActionDto param)
        {
            IGenericRepository<MembershipAction> _membershipActionRepo = _unitOfWork.MembershipActionRepository;

            var membershipActionEntity = _mapper.Map<MembershipAction>(param);
            _membershipActionRepo.Add(membershipActionEntity);
            await _unitOfWork.SaveAsync();
            var response = _mapper.Map<MembershipActionDto>(membershipActionEntity);
            return response;
        }
        #endregion
        #region handle update promotiontier
        private async Task<PromotionTierDto> updateTier(PromotionTierDto param)
        {
            IGenericRepository<PromotionTier> _tierRepo = _unitOfWork.PromotionTierRepository;
            param.UpdDate = DateTime.Now;

            var entity = _mapper.Map<PromotionTier>(param);
            _tierRepo.Update(entity);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<PromotionTierDto>(entity);
        }
        #endregion
    }
}
