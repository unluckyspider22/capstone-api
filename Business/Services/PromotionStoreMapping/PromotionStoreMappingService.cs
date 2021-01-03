using Infrastructure.Models;
using System;
using Infrastructure.DTOs.PromotionStoreMapping;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using AutoMapper;

namespace ApplicationCore.Services
{
    public class PromotionStoreMappingService : BaseService<PromotionStoreMapping, PromotionStoreMappingDto>, IPromotionStoreMappingService
    {
        public PromotionStoreMappingService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
           
        }

        protected override IGenericRepository<PromotionStoreMapping> _repository => _unitOfWork.PromotionStoreMappingRepository;
    }
}
