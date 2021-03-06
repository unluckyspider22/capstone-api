﻿using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IProductCateService : IBaseService<ProductCategory, ProductCategoryDto>
    {

        public Task<bool> CheckExistin(string cateId, Guid brandId);
        public Task<ProductCategoryDto> Update(ProductCategoryDto dto);
    }
}
