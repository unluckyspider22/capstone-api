using Infrastructure.Models;
using Infrastructure.DTOs;
using AutoMapper;
using Infrastructure.UnitOrWork;
using Infrastructure.Repository;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Services
{
    public class StoreService : BaseService<Store, StoreDto>, IStoreService
    {
        public StoreService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        protected override IGenericRepository<Store> _repository => _unitOfWork.StoreRepository;

        public async Task<List<GroupStoreOfPromotion>> GetStoreOfPromotion(Guid promotionId, Guid brandId)
        {
            IGenericRepository<PromotionStoreMapping> mappRepo = _unitOfWork.PromotionStoreMappingRepository;
            // Lấy danh sách store của cửa hàng
            var brandStore = (await _repository.Get(filter: el => el.BrandId.Equals(brandId) && !el.DelFlg)).ToList();
            // Lấy danh sách store của promotion
            var promoStore = (await mappRepo.Get(filter: el => el.PromotionId.Equals(promotionId), includeProperties: "Store")).ToList();
            // Map data cho reponse
            var mappResult = _mapper.Map<List<StoreOfPromotion>>(brandStore);
            foreach (var store in mappResult)
            {
                var strs = promoStore.Where(s => s.StoreId.Equals(store.StoreId));

                if (strs.Count() > 0)
                {
                    store.IsCheck = true;
                }
            }
            // Group các store
            var result = new List<GroupStoreOfPromotion>();
            var groups = mappResult.GroupBy(el => el.Group).Select(el => el.Distinct()).ToList();
            foreach (var group in groups)
            {

                var listStore = group.ToList();
                var groupStore = new GroupStoreOfPromotion
                {
                    Stores = listStore,
                    Group = listStore.First().Group
                };
                result.Add(groupStore);
            }
            return result;
        }
    }

}



