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
    public class PostActionService : BaseService<PostAction, PostActionDto>, IPostActionService
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

        public async Task<PostActionDto> MyAddAction(PostActionDto dto)
        {
            try
            {
                dto.PostActionId = Guid.NewGuid();
                dto.InsDate = DateTime.Now;
                dto.UpdDate = DateTime.Now;
                var entity = _mapper.Map<PostAction>(dto);
                _repository.Add(entity);
                if (dto.DiscountType == (int)AppConstant.EnvVar.PostActionType.Gift_Product && dto.ListProduct.Count > 0)
                {
                    IGenericRepository<PostActionProductMapping> mappRepo = _unitOfWork.PostActionProductMappingRepository;
                    foreach (var product in dto.ListProduct)
                    {
                        var mapp = new PostActionProductMapping()
                        {
                            Id = Guid.NewGuid(),
                            PostActionId = dto.PostActionId,
                            ProductId = product.ProductId,
                            InsDate = DateTime.Now,
                            UpdDate = DateTime.Now,
                            Quantity = product.Quantity,

                        };
                        mappRepo.Add(mapp);
                    }
                }
                await _unitOfWork.SaveAsync();
                return _mapper.Map<PostActionDto>(entity);
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
