

using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
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

        public async Task<ActionDto> MyAddAction(ActionDto dto)
        {
            try
            {
                dto.ActionId = Guid.NewGuid();
                dto.InsDate = DateTime.Now;
                dto.UpdDate = DateTime.Now;
                var entity = _mapper.Map<Infrastructure.Models.Action>(dto);
                _repository.Add(entity);
                if (dto.ListProduct.Count > 0 && dto.ActionType > (int)AppConstant.EnvVar.ActionType.Shipping)
                {
                    IGenericRepository<ActionProductMapping> mappRepo = _unitOfWork.ActionProductMappingRepository;
                    foreach (var productId in dto.ListProduct)
                    {
                        var mapp = new ActionProductMapping()
                        {
                            ActionId = dto.ActionId,
                            Id = Guid.NewGuid(),
                            InsDate = DateTime.Now,
                            UpdDate = DateTime.Now,
                            ProductId = productId,
                        };
                        mappRepo.Add(mapp);
                    }
                }
                await _unitOfWork.SaveAsync();
                return _mapper.Map<ActionDto>(entity);

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
