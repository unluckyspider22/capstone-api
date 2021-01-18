
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;

namespace ApplicationCore.Services
{
    public class MembershipActionService : BaseService<MembershipAction, MembershipActionDto>, IMembershipActionService
    {
        public MembershipActionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<MembershipAction> _repository => _unitOfWork.MembershipActionRepository;
    }
}
