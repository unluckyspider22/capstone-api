
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

        public async Task<List<PromotionTier>> GetActionCondition(Guid promotionId)
        {
            //List<PromotionTierResponse> result = new List<PromotionTierResponse>();

            IGenericRepository<PromotionTier> _tierRepo = _unitOfWork.PromotionTierRepository;
            IGenericRepository<ConditionRule> _conditionRuleRepo = _unitOfWork.ConditionRuleRepository;
            IGenericRepository<Infrastructure.Models.Action> _actionRepo = _unitOfWork.ActionRepository;
            IGenericRepository<MembershipAction> _membershipActionRepo = _unitOfWork.MembershipActionRepository;
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
    }
}
