using ApplicationCore.Models.PromotionTier;
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
    public class PromotionTierService : BaseService<PromotionTier, PromotionTierDto>, IPromotionTierService
    {
        public PromotionTierService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        protected override IGenericRepository<PromotionTier> _repository => _unitOfWork.PromotionTierRepository;
    }

}

