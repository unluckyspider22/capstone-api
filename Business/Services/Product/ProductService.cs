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

        public async Task<List<BrandProductDto>> GetAllBrandProduct(Guid brandId)
        {
            try
            {
                IGenericRepository<ProductCategory> cateRepo = _unitOfWork.ProductCategoryRepository;
                var result = new List<BrandProductDto>();
                var categories = (await cateRepo.Get(filter: o => o.BrandId.Equals(brandId)
                && !o.DelFlg, includeProperties: "Product")).ToList();
                if (categories != null && categories.Count > 0)
                {
                    foreach (var cate in categories)
                    {
                        var products = cate.Product.ToList();
                        if (products != null && products.Count > 0)
                        {
                            foreach (var product in products)
                            {
                                var dto = new BrandProductDto()
                                {
                                    CateId = cate.CateId,
                                    CateName = cate.Name,
                                    ProductCateId = cate.ProductCateId,
                                    Code = product.Code,
                                    ProductId = product.ProductId,
                                    ProductName = product.Name

                                };
                                result.Add(dto);
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at getVoucherForGame: \n" + e.StackTrace);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }
        }

        public async Task<GenericRespones<BrandProductDto>> GetBrandProduct(int PageSize, int PageIndex, Guid brandId)
        {
            try
            {
                IGenericRepository<ProductCategory> cateRepo = _unitOfWork.ProductCategoryRepository;
                var result = new List<BrandProductDto>();
                //var categories = (await cateRepo.Get(pageIndex: PageIndex, pageSize: PageSize,
                //    filter: o => o.BrandId.Equals(brandId) && !o.DelFlg, includeProperties: "Product")).ToList();
                //if (categories != null && categories.Count > 0)
                //{
                //    foreach (var cate in categories)
                //    {
                //        var products = cate.Product.ToList();
 
                //    }
                //}
                var products = (await _repository.Get(pageIndex: PageIndex, pageSize: PageSize,
                    filter: o => !o.DelFlg && o.ProductCate.BrandId.Equals(brandId),includeProperties: "ProductCate")).ToList();
                if (products != null && products.Count > 0)
                {
                    foreach (var product in products)
                    {
                        if (!product.DelFlg)
                        {
                            var dto = new BrandProductDto()
                            {
                                CateId = product.ProductCate.CateId,
                                CateName = product.ProductCate.Name,
                                ProductCateId = product.ProductCate.ProductCateId,
                                Code = product.Code,
                                ProductId = product.ProductId,
                                ProductName = product.Name

                            };
                            result.Add(dto);
                        }
                    }
                }
                var list = result;
                var totalItem = await _repository.CountAsync(filter: o => o.ProductCate.BrandId.Equals(brandId) && !o.DelFlg);
                MetaData metadata = new MetaData(pageIndex: PageIndex, pageSize: PageSize, totalItems: totalItem);
                GenericRespones<BrandProductDto> reponse = new GenericRespones<BrandProductDto>(data: list.ToList(), metadata: metadata);
                return reponse;
            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at getVoucherForGame: \n" + e.StackTrace);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }

        }

        public async Task<ProductDto> Update(ProductDto dto)
        {
            try
            {
                var entity = await _repository.GetFirst(filter: o => o.ProductId.Equals(dto.ProductId) && !o.DelFlg);
                if (entity != null)
                {
                    entity.UpdDate = DateTime.Now;
                    if (dto.CateId != null)
                    {
                        entity.CateId = dto.CateId;
                    }
                    if (dto.Name != null)
                    {
                        entity.Name = dto.Name;
                    }
                    if (dto.Code != null)
                    {
                        entity.Code = dto.Code;
                    }
                    if (dto.DelFlg != null)
                    {
                        entity.DelFlg = dto.DelFlg;
                    }
                    if (!dto.ProductCateId.Equals(Guid.Empty) && !dto.ProductCateId.Equals(entity.ProductCateId))
                    {
                        IGenericRepository<ProductCategory> cateRepo = _unitOfWork.ProductCategoryRepository;
                        var oldCate = await cateRepo.GetFirst(filter: o => o.ProductCateId.Equals(entity.ProductCateId) && !o.DelFlg);
                        oldCate.Product.Remove(entity);
                        var newCate = await cateRepo.GetFirst(filter: o => o.ProductCateId.Equals(dto.ProductCateId) && !o.DelFlg);
                        newCate.Product.Add(entity);
                        entity.ProductCateId = dto.ProductCateId;
                        entity.CateId = newCate.CateId;
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
