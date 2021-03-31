using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.DTOs.Support;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ProductService : BaseService<Product, ProductDto>, IProductService
    {
        //IProductCateService _cateService;
        //const string PASSIO_PRODUCT_HOST = "http://localhost:6789/localservice/getproducts";
        //const string PASSIO_LOGIN_HOST = "http://localhost:6789/localservice/login";

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IProductCateService cateService) : base(unitOfWork, mapper)
        {
            //this._cateService = cateService;
        }

        protected override IGenericRepository<Product> _repository => _unitOfWork.ProductRepository;


        protected IGenericRepository<ProductCategory> _cateRepos => _unitOfWork.ProductCategoryRepository;
        public async Task<bool> CheckExistin(string code, Guid brandId, Guid productId)
        {
            try
            {
                var isExist = false;
                if (productId != Guid.Empty)
                {
                    isExist = (await _repository.Get(filter: o => o.Code.Equals(code)
                          && o.ProductCate.BrandId.Equals(brandId)
                          && !o.ProductId.Equals(productId)
                          && !o.DelFlg)).ToList().Count > 0;
                }
                else
                {
                    isExist = (await _repository.Get(filter: o => o.Code.Equals(code)
                          && o.ProductCate.BrandId.Equals(brandId)
                          && !o.DelFlg)).ToList().Count > 0;
                }

                return isExist;
            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at getVoucherForGame: \n" + e.StackTrace);
                throw new ErrorObj(code: 500, message: e.Message);
            }

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
                throw new ErrorObj(code: 500, message: e.Message);
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
                    filter: o => !o.DelFlg && o.ProductCate.BrandId.Equals(brandId), includeProperties: "ProductCate")).ToList();
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
                var totalItem = (await _repository.Get(filter: o => !o.DelFlg, includeProperties: "ProductCate")).Where(o => o.ProductCate.BrandId.Equals(brandId)).ToList().Count();
                MetaData metadata = new MetaData(pageIndex: PageIndex, pageSize: PageSize, totalItems: totalItem);
                GenericRespones<BrandProductDto> reponse = new GenericRespones<BrandProductDto>(data: list.ToList(), metadata: metadata);
                return reponse;
            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at getVoucherForGame: \n" + e.StackTrace);
                throw new ErrorObj(code: 500, message: e.Message);
            }

        }

        public async Task addCateFromSync(Guid brandId, ProductSyncParamDTO productSyncs)
        {
            var listCateBefore = _cateRepos.Get(filter: el => el.BrandId.Equals(brandId) && !el.DelFlg).Result.ToList();
            foreach (CategorySync cateSync in productSyncs.data.Categories)
            {
                if (listCateBefore.Any(a => a.CateId.Equals(cateSync.Id.ToString())))
                {
                    var productCate = listCateBefore.Where(w => w.CateId.Equals(cateSync.Id.ToString())).First();
                    productCate.Name = cateSync.Name;
                    _cateRepos.Update(productCate);

                }
                else
                {
                    ProductCategoryDto cateDto = new ProductCategoryDto
                    {
                        ProductCateId = Guid.NewGuid(),
                        BrandId = brandId,
                        CateId = cateSync.Id.ToString(),
                        Name = cateSync.Name,
                        InsDate = DateTime.Now,
                        UpdDate = DateTime.Now
                    };

                    _cateRepos.Add(_mapper.Map<ProductCategory>(cateDto));
                }
            }
            await _unitOfWork.SaveAsync();
        }
        public async Task addProductFromSync(Guid brandId, ProductSyncParamDTO productSyncs)
        {
            var listProduct = _repository.Get(filter: el => !el.DelFlg && el.ProductCate.BrandId.Equals(brandId)).Result.ToList();
            var listCateAfter = _cateRepos.Get(filter: el => el.BrandId.Equals(brandId) && !el.DelFlg).Result.ToList();
            foreach (var productSync in productSyncs.data.Products)
            {
                if (productSync.ChildrenProducts.Count < 1 || productSync.ChildrenProducts == null)
                {
                    if (!listProduct.Any(a => a.Code.Equals(productSync.Code)))
                    {
                        var existCate = listCateAfter.Where(w => w.CateId.Equals(productSync.CateId.ToString())).ToList();
                        if (existCate != null && existCate.Count > 0)
                        {
                            ProductDto productDto = new ProductDto
                            {
                                CateId = productSync.CateId.ToString(),
                                Name = productSync.ProductName,
                                Code = productSync.Code,
                                ProductId = Guid.NewGuid(),
                                InsDate = DateTime.Now,
                                UpdDate = DateTime.Now,
                                ProductCateId = existCate.First().ProductCateId
                            };
                            _repository.Add(_mapper.Map<Product>(productDto));
                        }
                        //await _unitOfWork.SaveAsync();
                    }
                    else
                    {
                        var product = listProduct.Where(w => w.Code.Equals(productSync.Code)).First();
                        product.Name = productSync.ProductName;
                        _repository.Update(product);
                    }

                }
                else
                {
                    foreach (var child in productSync.ChildrenProducts)
                    {
                        if (!listProduct.Any(a => a.Code.Equals(child.Code)))
                        {
                            var existCate = listCateAfter.Where(w => w.CateId.Equals(child.CatID.ToString())).ToList();
                            if (existCate != null && existCate.Count > 0)
                            {
                                ProductDto productDto = new ProductDto
                                {
                                    CateId = child.CatID.ToString(),
                                    Name = child.ProductName,
                                    Code = child.Code,
                                    ProductId = Guid.NewGuid(),
                                    InsDate = DateTime.Now,
                                    UpdDate = DateTime.Now,
                                    ProductCateId = existCate.First().ProductCateId
                                };

                                _repository.Add(_mapper.Map<Product>(productDto));
                            }

                        }
                        else
                        {
                            var product = listProduct.Where(w => w.Code.Equals(child.Code)).First();
                            product.Name = productSync.ProductName;
                            _repository.Update(product);
                        }
                    }
                }

            }
            await _unitOfWork.SaveAsync();
        }
        public async Task<ProductSyncParamDTO> SyncProduct(
            Guid brandId, ProductRequestParam productRequestParam)
        {

            try
            {
                var productSyncs = await getProductFromApiUrl(productRequestParam);
                //var cateList = _cateRepos.Get(filter: el => el.BrandId.Equals(brandId)).Result.ToList();
                await addCateFromSync(brandId, productSyncs);
                await addProductFromSync(brandId, productSyncs);
                return productSyncs;
            }
            catch (Exception e)
            {
                Debug.WriteLine("\n\nError at getVoucherForGame: \n" + e.Message);
                throw new ErrorObj(code: 500, message: e.Message);
            }

        }
        private async Task<ProductSyncParamDTO> getProductFromApiUrl(ProductRequestParam productRequestParam)
        {
            try
            {
                var token = await getToken(productRequestParam);
                ProductSyncParamDTO productSyncParamDTO = null;
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                //var url = (PASSIO_PRODUCT_HOST);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(productRequestParam.SyncUrl),
                    //Content = new StringContent(json, Encoding.UTF8, "application/json"),
                };
                request.Headers.TryAddWithoutValidation("Authorization", String.Format("Bearer {0}", token));
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    response.Content.Headers.ContentType.CharSet = @"utf-8";
                    var responseBody = await response.Content.ReadAsStringAsync();
                    productSyncParamDTO = JsonConvert.DeserializeObject<ProductSyncParamDTO>(responseBody);
                }
                return productSyncParamDTO;
            }
            catch (Exception e)
            {
                Debug.WriteLine("\n\nError at getVoucherForGame: \n" + e.Message);
                throw new ErrorObj(code: 500, message: e.Message);
            }

        }
        private async Task<string> getToken(ProductRequestParam productRequestParam)
        {
            try
            {
                string bearerToken = "";
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                var json = "{" + string.Format("\"username\":\"{0}\"," +
                                               "\"password\":\"{1}\"," +
                                               "\"device_id\":\"{2}\"",
                                               productRequestParam.TokenBody.UserName,
                                               productRequestParam.TokenBody.PassWord,
                                               productRequestParam.TokenBody.Device_Id) + "}";
                Debug.WriteLine(json);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(productRequestParam.TokenUrl),
                    Content = new StringContent(json, Encoding.UTF8, "application/json"),
                };
                HttpResponseMessage bearerResult = await client.SendAsync(request);

                if (bearerResult.IsSuccessStatusCode)
                {
                    var bearerData = await bearerResult.Content.ReadAsStringAsync();
                    JObject jObject = JObject.Parse(bearerData);
                    TokenData tokenData = jObject.ToObject<TokenData>();
                    bearerToken = tokenData.data.Token;
                }
                return bearerToken;
            }
            catch (Exception e)
            {
                Debug.WriteLine("\n\nError at getVoucherForGame: \n" + e.Message);
                throw new ErrorObj(code: 500, message: e.Message);

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
                    if (dto.Name != null)
                    {
                        entity.Name = dto.Name;
                    }
                    if (dto.Code != null)
                    {
                        entity.Code = dto.Code;
                    }
                    if (!dto.ProductCateId.Equals(Guid.Empty) && !dto.ProductCateId.Equals(entity.ProductCateId))
                    {
                        IGenericRepository<ProductCategory> cateRepo = _unitOfWork.ProductCategoryRepository;
                        var oldCate = await cateRepo.GetFirst(filter: o => o.ProductCateId.Equals(entity.ProductCateId) && !o.DelFlg);
                        oldCate.Product.Remove(entity);
                        var newCate = await cateRepo.GetFirst(filter: o => o.ProductCateId.Equals(dto.ProductCateId) && !o.DelFlg);
                        newCate.Product.Add(entity);
                        entity.ProductCateId = dto.ProductCateId;
                        /*entity.CateId = newCate.CateId;*/
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
                throw new ErrorObj(code: 500, message: e.Message);
            }
        }
    }
}
