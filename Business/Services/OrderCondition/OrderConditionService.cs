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
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class OrderConditionService : BaseService<OrderCondition, OrderConditionDto>, IOrderConditionService
    {
        public OrderConditionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<OrderCondition> _repository => _unitOfWork.OrderConditionRepository;
    }
}
