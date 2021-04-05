﻿using Infrastructure.Models;
using Infrastructure.DTOs;
using AutoMapper;
using Infrastructure.UnitOrWork;
using Infrastructure.Repository;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.DTOs.Voucher;
using Infrastructure.Helper;

namespace ApplicationCore.Services
{
    public class StoreService : BaseService<Store, StoreDto>, IStoreService
    {
        public StoreService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        protected override IGenericRepository<Store> _repository => _unitOfWork.StoreRepository;

        public async Task<List<PromotionInfomation>> GetPromotionsForStore(string brandCode, string storeCode)
        {
            List<PromotionInfomation> result = null;
            var store = await _repository.GetFirst(filter: el =>
                    !el.DelFlg
                    && el.StoreCode.Equals(storeCode)
                    && el.Brand.BrandCode.Equals(brandCode),
                includeProperties: "Brand.Promotion," +
                "PromotionStoreMapping.Promotion");

            var promotions = store.PromotionStoreMapping.Where(w => w.Store.StoreCode.Equals(storeCode)
                    && w.Promotion.Status == (int)AppConstant.EnvVar.PromotionStatus.PUBLISH)
                    .Where(w => DateTime.Now <= (w.Promotion.EndDate != null ? w.Promotion.EndDate : DateTime.MaxValue))
                        .Select(s => s.Promotion);

            if (promotions != null && promotions.Count() > 0)
            {
                foreach (var promotion in promotions)
                {
                    var promotionInfomation = _mapper.Map<PromotionInfomation>(promotion);
                    if (result == null)
                    {
                        result = new List<PromotionInfomation>();
                    }
                    result.Add(promotionInfomation);
                }
            }

            return result;
        }

        public async Task<List<GroupStoreOfPromotion>> GetStoreOfPromotion(Guid promotionId, Guid brandId)
        {
            try
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
            catch (Exception e)
            {
                throw new ErrorObj(code: 500, message: e.Message, description: "Internal Server Error");
            }
        }

        public async Task<List<GroupStoreOfPromotion>> UpdateStoreOfPromotion(UpdateStoreOfPromotion dto)
        {
            try
            {
                IGenericRepository<PromotionStoreMapping> mappRepo = _unitOfWork.PromotionStoreMappingRepository;

                // Xóa data trong bảng store mapping
                mappRepo.Delete(id: Guid.Empty, filter: el => el.PromotionId.Equals(dto.PromotionId));

                // Insert data mới vào bảng store mapping
                var stores = dto.ListStoreId;
                foreach (var store in stores)
                {
                    PromotionStoreMapping obj = new PromotionStoreMapping
                    {
                        Id = Guid.NewGuid(),
                        PromotionId = dto.PromotionId,
                        StoreId = store,
                        InsDate = DateTime.Now,
                        UpdDate = DateTime.Now
                    };
                    mappRepo.Add(obj);
                }

                await _unitOfWork.SaveAsync();
                var result = await GetStoreOfPromotion(brandId: dto.BrandId, promotionId: dto.PromotionId);

                return result;
            }
            catch (Exception e)
            {
                throw new ErrorObj(code: 500, message: e.Message, description: "Internal Server Error");
            }

        }
    }

}



