using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IProductService : IBaseService<Product,ProductDto>
    {
        public Task<bool> CheckExistin(string code, string cateId, Guid productCateId);
        public Task<ProductDto> Update(ProductDto dto);
    }
}
