using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;

namespace ApplicationCore.Services
{
    public class MembershipConditionService : BaseService<MembershipCondition, MembershipConditionDto>, IMembershipConditionService
    {
        public MembershipConditionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<MembershipCondition> _repository => _unitOfWork.MembershipConditionRepository;
    }
}
