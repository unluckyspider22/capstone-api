

using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;

namespace ApplicationCore.Services
{
    public class ActionService : BaseService<Action, ActionDto>, IActionService
    {
        public ActionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Action> _repository => _unitOfWork.ActionRepository;
    }
}
