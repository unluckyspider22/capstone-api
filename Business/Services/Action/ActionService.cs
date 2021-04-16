

using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Collections.Generic;
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

        public async Task<ActionDto> GetActionDetail(Guid id)
        {
            var result = new ActionDto();
            var entity = await _repository.GetFirst(filter: o => o.ActionId.Equals(id), includeProperties: "ActionProductMapping");
            if (entity != null)
            {
                result = _mapper.Map<ActionDto>(entity);
                result.ListProduct = new List<ActionProductMap>();
                if (entity.ActionProductMapping.Count > 0)
                {
                    foreach (var product in entity.ActionProductMapping)
                    {
                        var dto = new ActionProductMap()
                        {
                            ProductId = product.ProductId,
                            Quantity = (int)product.Quantity,
                        };
                        result.ListProduct.Add(dto);
                    }
                }

            }
            return result;

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
                    foreach (var product in dto.ListProduct)
                    {
                        var mapp = new ActionProductMapping()
                        {
                            ActionId = dto.ActionId,
                            Id = Guid.NewGuid(),
                            InsDate = DateTime.Now,
                            UpdDate = DateTime.Now,
                            ProductId = product.ProductId,
                            Quantity = product.Quantity,
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

        //public Task<ActionDto> UpdateAction(ActionDto dto)
        //{

        //}
    }
}
