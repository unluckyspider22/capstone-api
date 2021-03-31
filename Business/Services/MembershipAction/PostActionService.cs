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
    public class PostActionService : BaseService<PostAction, MembershipActionDto>, IPostActionService
    {
        public PostActionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<PostAction> _repository => _unitOfWork.PostActionRepository;

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                IGenericRepository<PostActionProductMapping> mappRepo = _unitOfWork.PostActionProductMappingRepository;
                mappRepo.Delete(id: Guid.Empty, filter: o => o.PostActionId.Equals(id));
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
