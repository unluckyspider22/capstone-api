using AutoMapper;
using Infrastructure.DTOs;
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
    public class ProductService : BaseService<Product, ProductDto>, IProductService
    {
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Product> _repository => _unitOfWork.ProductRepository;

        public async Task<bool> CheckExistin(string code, string cateId, Guid productCateId)
        {
            var isExist = (await _repository.Get(filter: o => o.Code.Equals(code) 
            && o.ProductCateId.Equals(productCateId) 
            && o.CateId.Equals(cateId)
            && !o.DelFlg)).ToList().Count > 0;
            return isExist;
        }

        public async Task<ProductDto> Update(ProductDto dto)
        {
            try
            {
                var entity = await _repository.GetFirst(filter: o => o.ProductId.Equals(dto.ProductId) && !o.DelFlg);
                if (entity != null)
                {
                    entity.UpdDate = DateTime.Now;
                    var updParam = _mapper.Map<Product>(dto);
                    if (updParam.CateId != null)
                    {
                        entity.CateId = dto.CateId;
                    }
                    if (updParam.Name != null)
                    {
                        entity.Name = dto.Name;
                    }
                    if (updParam.Code != null)
                    {
                        entity.Code = dto.Code;
                    }
                    _repository.Update(entity);
                    await _unitOfWork.SaveAsync();
                    return _mapper.Map<ProductDto>(entity);
                }
                else
                {
                    throw new ErrorObj(code: 500, message: "Product category not found");
                }
            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at getVoucherForGame: \n" + e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }
        }
    }
}
