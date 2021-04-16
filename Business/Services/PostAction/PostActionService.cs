using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class GiftService : BaseService<Gift, GiftDto>, IGiftService
    {
        public GiftService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Gift> _repository => _unitOfWork.GiftRepository;

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                IGenericRepository<GiftProductMapping> mappRepo = _unitOfWork.GiftProductMappingRepository;
                mappRepo.Delete(id: Guid.Empty, filter: o => o.GiftId.Equals(id));
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

        public async Task<GiftDto> GetGiftDetail(Guid id)
        {
            var result = new GiftDto();
            var entity = await _repository.GetFirst(filter: o => o.GiftId.Equals(id), includeProperties: "GiftProductMapping");
            if (entity != null)
            {
                result = _mapper.Map<GiftDto>(entity);
                result.ListProductMapp = new List<GiftProductMapping>();
                if (entity.GiftProductMapping.Count > 0)
                {
                    result.ListProductMapp = entity.GiftProductMapping.ToList();
                }
            }
            return result;
        }

        public async Task<GiftDto> MyAddAction(GiftDto dto)
        {
            try
            {
                dto.GiftId = Guid.NewGuid();
                dto.InsDate = DateTime.Now;
                dto.UpdDate = DateTime.Now;
                var entity = _mapper.Map<Gift>(dto);
                _repository.Add(entity);
                if (dto.PostActionType == (int)AppConstant.EnvVar.PostActionType.Gift_Product && dto.ListProduct.Count > 0)
                {
                    IGenericRepository<GiftProductMapping> mappRepo = _unitOfWork.GiftProductMappingRepository;
                    foreach (var product in dto.ListProduct)
                    {
                        var mapp = new GiftProductMapping()
                        {
                            Id = Guid.NewGuid(),
                            GiftId = dto.GiftId,
                            ProductId = product.ProductId,
                            InsDate = DateTime.Now,
                            UpdDate = DateTime.Now,
                            Quantity = product.Quantity,

                        };
                        mappRepo.Add(mapp);
                    }
                }
                await _unitOfWork.SaveAsync();
                return _mapper.Map<GiftDto>(entity);
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
