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
        IProductCateService _cateService;
        const string PASSIO_PRODUCT_HOST = "http://localhost:6789/localservice/getproducts";
        const string PASSIO_LOGIN_HOST = "http://localhost:6789/localservice/login";

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IProductCateService cateService) : base(unitOfWork, mapper)
        {
            this._cateService = cateService;
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

        public async Task<ProductSyncParamDTO> SyncProduct(Guid brandId, ProductRequestParam productRequestParam)
        {
            //List<ProductCategoryDto> listCate = new List<ProductCategoryDto>();
            //List<ProductDto> listProduct = new List<ProductDto>();
            try
            {
                var result = await getProductFromApiUrl(productRequestParam);
                var listCate = await _cateService.GetAll(brandId);
                foreach (CategorySync cateSync in result.data.Categories)
                {
                    ProductCategoryDto cateDto = new ProductCategoryDto();
                    cateDto.ProductCateId = Guid.NewGuid();
                    cateDto.BrandId = brandId;
                    cateDto.CateId = cateSync.Id.ToString();
                    cateDto.Name = cateSync.Name;
                    cateDto.InsDate = DateTime.Now;
                    //if(listCate.Where(w => w.CateId.Equals(cateDto.CateId))))
                    await _cateService.CreateAsync(cateDto);
                }
                foreach (ProductSync productSync in result.data.Products)
                {
                    if (productSync.ChildrenProducts.Count < 1 || productSync.ChildrenProducts == null)
                    {
                        ProductDto productDto = new ProductDto();
                        productDto.CateId = productSync.CateId.ToString();
                        productDto.Name = productSync.ProductName;
                        productDto.Code = productSync.Code;
                        productDto.ProductId = Guid.NewGuid();
                        productDto.InsDate = DateTime.Now;
                        _repository.Add(_mapper.Map<Product>(productDto));
                    }
                    else
                    {
                        foreach (ChildrenProductSync childrenProductSync in productSync.ChildrenProducts)
                        {
                            ProductDto productDto = new ProductDto();
                            productDto.CateId = childrenProductSync.CatID.ToString();
                            productDto.Name = childrenProductSync.ProductName;
                            productDto.Code = childrenProductSync.Code;
                            productDto.ProductId = Guid.NewGuid();
                            productDto.InsDate = DateTime.Now;
                            _repository.Add(_mapper.Map<Product>(productDto));
                        }
                    }
                }
                return result;
            }
            catch (Exception e)
            {
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
                //var encodedPair = Base64Encode(String.Format("{0}:{1}", encodedConsumerKey, encodedConsumerKeySecret));
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
                    //productRequestParam.TokenUrl
                    RequestUri = new Uri(productRequestParam.TokenUrl),
                    Content = new StringContent(json, Encoding.UTF8, "application/json"),                  
                };
                //requestToken.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded") { CharSet = "UTF-8" };
                //requestToken.Headers.TryAddWithoutValidation("Authorization", String.Format("Basic {0}", encodedPair));
                //request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded") { CharSet = "UTF-8" };
                HttpResponseMessage bearerResult = await client.SendAsync(request);

                if (bearerResult.IsSuccessStatusCode)
                {
                    var bearerData = await bearerResult.Content.ReadAsStringAsync();
                    JObject jObject = JObject.Parse(bearerData);
                    TokenData tokenData = jObject.ToObject<TokenData>();
                    bearerToken = tokenData.data.Token;
                    //var responseBody = await response.Content.ReadAsStringAsync();
                    //productSyncParamDTO = JsonConvert.DeserializeObject<ProductSyncParamDTO>(responseBody);
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
