
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.DTOs.Voucher;
using Infrastructure.DTOs.VoucherChannel;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
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

        public async Task<VoucherForChannelResponse> GetVouchersForChannel(VoucherChannelParam channelParam)
        {
            VoucherForChannelResponse result = new VoucherForChannelResponse();
            try
            {
                var promotion = await _promotionService
                    .GetFirst(el =>
                    el.PromotionId.Equals(channelParam.PromotionId),
                    includeProperties: "PromotionStoreMapping.Store," +
                    "VoucherGroup");

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
                    var vouchers = await _voucherService.GetVouchersForChannel(voucherGroup, channelParam);
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
    }
}
