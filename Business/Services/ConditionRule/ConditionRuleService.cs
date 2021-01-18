using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;

namespace ApplicationCore.Services
{
    public class ConditionRuleService : BaseService<ConditionRule, ConditionRuleDto>, IConditionRuleService
    {
        public ConditionRuleService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<ConditionRule> _repository => _unitOfWork.ConditionRuleRepository;
    }
}
