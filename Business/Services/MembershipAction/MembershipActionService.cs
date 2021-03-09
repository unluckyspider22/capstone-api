
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;

namespace ApplicationCore.Services
{
    public class MembershipActionService : BaseService<PostAction, MembershipActionDto>, IMembershipActionService
    {
        public MembershipActionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<PostAction> _repository => _unitOfWork.PostActionRepository;
    }
}
