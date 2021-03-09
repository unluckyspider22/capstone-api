
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.DTOs.Voucher;
using Infrastructure.DTOs.VoucherChannel;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ChannelService : BaseService<Channel, ChannelDto>, IChannelService
    {
        private readonly IVoucherService _voucherService;
        private readonly IPromotionService _promotionService;
        private readonly IVoucherGroupService _voucherGroupService;
        private readonly IStoreService _storeService;

        public ChannelService(IUnitOfWork unitOfWork, IMapper mapper,
            IVoucherService voucherService,
            IPromotionService promotionService,
            IVoucherGroupService voucherGroupService,
            IStoreService storeService) : base(unitOfWork, mapper)
        {
            _voucherService = voucherService;
            _promotionService = promotionService;
            _voucherGroupService = voucherGroupService;
            _storeService = storeService;
        }

        protected override IGenericRepository<Channel> _repository => _unitOfWork.ChannelRepository;

        public async Task<List<PromotionInfomation>> GetPromotionsForChannel(VoucherChannelParam channelParam)
        {
            var result = new List<PromotionInfomation>();
            try
            {
                var promotions = (await _promotionService.GetAsync(filter: el =>
                el.Status.Equals(AppConstant.EnvVar.PromotionStatus.PUBLISH)
                && !el.DelFlg,
                includeProperties:
                "Brand,PromotionChannelMapping.Channel")).Data; ;

                promotions = promotions.Where(w =>
                w.PromotionChannelMapping.Select(vc =>
                    vc.Channel).Any(a =>
                        a.ChannelCode.Equals(channelParam.ChannelCode)
                        && a.Brand.BrandCode.Equals(channelParam.BrandCode)
                        && !a.DelFlg)).ToList();
                foreach (var promotion in promotions)
                {
                    var promotionInfomation = _mapper.Map<PromotionInfomation>(promotion);

                    result.Add(promotionInfomation);
                }
                return result;
            }
            catch (ErrorObj er)
            {
                throw er;
            }
            catch (Exception e)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e.Message);
            }
        }

        public async Task<VoucherForChannelResponse> GetVouchersForChannel(VoucherChannelParam channelParam)
        {
            VoucherForChannelResponse result = new VoucherForChannelResponse();
            try
            {
                var promotion = await _promotionService
                    .GetFirst(el =>
                    el.PromotionId.Equals(channelParam.PromotionId),
                    includeProperties: "PromotionStoreMapping.Store," +
                    "VoucherGroup," +
                    "VoucherChannel.Channel");

                if (promotion != null)
                {
                    //Map thông tin promotion
                    result.PromotionData = _mapper.Map<PromotionInfomation>(promotion);
                    //Lấy thông tin của các cửa hàng được áp dụng
                    result.StoresData = promotion.PromotionStoreMapping.Select(s => s.Store.StoreName).ToList();
                    #region Xử lý lấy các cửa hàng được áp dụng
                    var storeOfBrand = await _storeService.GetAsync(filter: el => el.BrandId.Equals(promotion.BrandId) && !el.DelFlg);
                    if (storeOfBrand.Data.Count() == promotion.PromotionStoreMapping.Count())
                    {
                        result.StoreAppied = AppConstant.EnvVar.ApplyForAllStore;
                    }
                    else
                    {
                        foreach (var storeName in result.StoresData)
                        {
                            result.StoreAppied += storeName + ", ";
                        }
                    }
                    #endregion
                    var voucherGroup = promotion.VoucherGroup;
                    result.PromotionData.VoucherName = voucherGroup.VoucherName;
                    //Lấy voucherchannel để update khi lấy voucher
                    var voucherChannel = promotion.PromotionChannelMapping
                          .FirstOrDefault(w => w.Channel.ChannelCode.Equals(channelParam.ChannelCode));
                    //Lấy danh sách voucher và đánh dấu đã được lấy bởi channel nào
                    var vouchers = await _voucherService.GetVouchersForChannel(voucherChannel, voucherGroup, channelParam);
                    if (vouchers != null && vouchers.Count() > 0)
                    {
                        //Lấy danh sách voucher
                        result.Vouchers = vouchers.Select(s => promotion.PromotionCode + AppConstant.EnvVar.CONNECTOR + s.VoucherCode).ToList();
                        //Update số lượng voucher đã được Redemped
                        await _voucherGroupService.UpdateRedempedQuantity(voucherGroup, vouchers.Count());
                    }
                    else
                    {
                        throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Voucher_OutOfStock);
                    }
                }
                return result;
            }
            catch (ErrorObj e1)
            {
                throw e1;
            }
            catch (Exception e2)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e2.Message);
            }
        }

        #region lấy channel cho promotion
        public async Task<List<GroupChannelOfPromotion>> GetChannelOfPromotion(Guid promotionId, Guid brandId)
        {
            IGenericRepository<PromotionChannelMapping> mappRepo = _unitOfWork.VoucherChannelRepository;
            // Lấy danh sách channel của cửa hàng
            var brandChannel = (await _repository.Get(filter: el => el.BrandId.Equals(brandId) && !el.DelFlg)).ToList();
            // Lấy danh sách channel của promotion
            var promoChannel = (await mappRepo.Get(filter: el => el.PromotionId.Equals(promotionId), includeProperties: "Channel")).ToList();
            // Map data cho reponse
            var mappResult = _mapper.Map<List<ChannelOfPromotion>>(brandChannel);
            foreach (var channel in mappResult)
            {
                var strs = promoChannel.Where(s => s.ChannelId.Equals(channel.ChannelId));

                if (strs.Count() > 0)
                {
                    channel.IsCheck = true;
                }
            }
            // Group các store
            var result = new List<GroupChannelOfPromotion>();
            var groups = mappResult.GroupBy(el => el.Group).Select(el => el.Distinct()).ToList();
            foreach (var group in groups)
            {

                var listChannel = group.ToList();
                var groupChannel = new GroupChannelOfPromotion
                {
                    Channels = listChannel,
                    Group = listChannel.First().Group
                };
                result.Add(groupChannel);
            }
            return result;
        }

        public async Task<List<GroupChannelOfPromotion>> UpdateChannelOfPromotion(UpdateChannelOfPromotion dto)
        {
            try
            {
                IGenericRepository<PromotionChannelMapping> mappRepo = _unitOfWork.VoucherChannelRepository;

                // Xóa data trong bảng channel mapping
                mappRepo.Delete(id: Guid.Empty, filter: el => el.PromotionId.Equals(dto.PromotionId));

                // Insert data mới vào bảng store mapping
                var channels = dto.ListChannelId;
                foreach (var channel in channels)
                {
                    PromotionChannelMapping obj = new PromotionChannelMapping
                    {
                        VoucherChannelId = Guid.NewGuid(),
                        PromotionId = dto.PromotionId,
                        ChannelId = channel,
                        InsDate = DateTime.Now,
                        UpdDate = DateTime.Now
                    };
                    mappRepo.Add(obj);
                }

                await _unitOfWork.SaveAsync();
                var result = await GetChannelOfPromotion(brandId: dto.BrandId, promotionId: dto.PromotionId);

                return result;
            }
            catch (Exception e)
            {
                throw new ErrorObj(code: 500, message: e.Message, description: "Internal Server Error");
            }
        }
        #endregion
    }
}
