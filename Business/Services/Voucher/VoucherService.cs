
using ApplicationCore.Request;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.DTOs.VoucherChannel;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class VoucherService : BaseService<Voucher, VoucherDto>, IVoucherService
    {
        public VoucherService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        protected override IGenericRepository<Voucher> _repository => _unitOfWork.VoucherRepository;

        public async Task<int> ActiveAllVoucherInGroup(VoucherGroupDto Dto)
        {
            try
            {
                int result = 0;
                var listVoucher = await _repository.Get(filter: el => el.IsActive.Equals("0") || !el.IsActive
                && el.VoucherGroupId.Equals(Dto.VoucherGroupId));
                foreach (Voucher voucher in listVoucher.ToList())
                {
                    voucher.UpdDate = DateTime.Now;
                    voucher.IsActive = true;
                    _repository.Update(voucher);
                    await _unitOfWork.SaveAsync();
                    result++;
                }
                return result;
            }
            catch (Exception e)
            {

                Debug.WriteLine("\n\nError at activeAllVoucherInGroup: \n" + e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.", description: "Internal Server Error");
            }

        }

        public async Task<List<Promotion>> CheckVoucher(OrderResponseModel order)
        {
            try
            {
                var vouchers = order.Vouchers;
                if (vouchers.Select(el => new { el.VoucherCode, el.PromotionCode }).Distinct().Count() < vouchers.Select(el => new { el.VoucherCode, el.PromotionCode }).Count())
                {
                    throw new ErrorObj(code: 400, message: AppConstant.ErrMessage.Duplicate_VoucherCode, description: AppConstant.ErrMessage.Duplicate_VoucherCode);
                }
                var promotions = new List<Promotion>();
                foreach (VoucherResponseModel voucherModel in vouchers)
                {
                    // throw new ErrorObj(code: 400, message:"voucherCode: " + voucherModel.VoucherCode + ", promotionCode: " + voucherModel.PromotionCode, description: AppConstant.ErrMessage.Invalid_VoucherCode);
                    var voucher = await _repository.Get(filter: el => el.IsActive
                    && el.VoucherCode.Equals(voucherModel.VoucherCode)
                    && el.VoucherGroup.Promotion.PromotionCode.Equals(voucherModel.PromotionCode)
                    && !el.IsUsed,
                    includeProperties:
                    "VoucherGroup.Promotion.PromotionTier.Action," +
                    "VoucherGroup.Promotion.PromotionTier.ConditionRule.ConditionGroup.OrderCondition," +
                    "VoucherGroup.Promotion.PromotionTier.ConditionRule.ConditionGroup.ProductCondition," +
                    "VoucherGroup.Promotion.PromotionTier.ConditionRule.ConditionGroup.MembershipCondition," +
                    "VoucherGroup.Promotion.PromotionStoreMapping.Store");
                    //  throw new ErrorObj(code: 400, message:"count: " +voucher.Count(), description: AppConstant.ErrMessage.Invalid_VoucherCode);

                    if (voucher.Count() > 1)
                    {
                        throw new ErrorObj(code: 400, message: AppConstant.ErrMessage.Duplicate_VoucherCode, description: AppConstant.ErrMessage.Duplicate_VoucherCode);
                    }
                    if (voucher.Count() == 0)
                    {
                        throw new ErrorObj(code: 400, message: AppConstant.ErrMessage.Invalid_VoucherCode, description: AppConstant.ErrMessage.Invalid_VoucherCode);
                    }
                    var promotion = voucher.First().VoucherGroup.Promotion;
                    promotions.Add(promotion);
                }
                return promotions;
            }
            catch (ErrorObj e1)
            {
                throw e1;
            }
            catch (Exception e)
            {

                throw new ErrorObj(code: 500, message: e.Message, description: "Internal Server Error");
            }


        }

        public async Task<List<Voucher>> GetVouchersForChannel(VoucherGroup voucherGroup, VoucherChannelParam channelParam)
        {

            int remainVoucher = (int)(voucherGroup.Quantity - voucherGroup.RedempedQuantity);
            if (remainVoucher == 0)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Voucher_OutOfStock);
            }
            if (channelParam.Quantity > remainVoucher)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_VoucherQuantity + remainVoucher);
            }

            var vouchers = await _repository.Get(
                pageSize: channelParam.Quantity,
                pageIndex: 1,
                filter: el =>
                    el.VoucherGroup.IsActive
                   && !el.IsRedemped
                   && !el.IsUsed
                   && el.VoucherGroupId.Equals(voucherGroup.VoucherGroupId));
            if (vouchers.Count() > 0)
            {
                foreach (var voucher in vouchers)
                {
                    voucher.IsRedemped = true;
                    var now = DateTime.Now;
                    voucher.RedempedDate = now;
                    voucher.UpdDate = now;
                    _repository.Update(voucher);
                }
                await _unitOfWork.SaveAsync();
            }
            return vouchers.ToList();
        }
    }
}

