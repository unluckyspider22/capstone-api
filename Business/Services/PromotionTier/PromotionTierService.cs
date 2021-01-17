
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;

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

