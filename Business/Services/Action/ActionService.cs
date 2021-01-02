
using ApplicationCore.Models;
using ApplicationCore.Utils;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;

namespace ApplicationCore.Services
{
    public class ActionService : BaseService<Infrastructure.Models.Action, ActionDto>, IActionService
    {
        public ActionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Action> _repository => throw new System.NotImplementedException();
    }
}
