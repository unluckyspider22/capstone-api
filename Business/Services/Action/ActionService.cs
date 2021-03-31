

using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ActionService : BaseService<Infrastructure.Models.Action, ActionDto>, IActionService
    {
        public ActionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Infrastructure.Models.Action> _repository => _unitOfWork.ActionRepository;

        public async Task<bool> Delete(System.Guid id)
        {
            try
            {
                IGenericRepository<ActionProductMapping> mappRepo = _unitOfWork.ActionProductMappingRepository;
                mappRepo.Delete(id: Guid.Empty, filter: o => o.ActionId.Equals(id));
                _repository.Delete(id: id);
                return await _unitOfWork.SaveAsync() > 0;
            }

            catch (System.Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.InnerException);
                throw new ErrorObj(code: 500, message: e.Message, description: "Internal Server Error");
            }
        }
    }
}
