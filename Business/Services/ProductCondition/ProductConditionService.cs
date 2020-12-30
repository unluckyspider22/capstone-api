using ApplicationCore.Models;
using ApplicationCore.Utils;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Services
{
    public class ProductConditionService : BaseService<ProductCondition, ProductConditionDto>, IProductConditionService
    {
        public ProductConditionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<ProductCondition> _repository => _unitOfWork.ProductConditionRepository;
    }
}
