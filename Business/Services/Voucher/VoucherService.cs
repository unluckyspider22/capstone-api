
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
        IVoucherGroupService _voucherGroupService;
        IMembershipService _membershipService;

        public VoucherService(IUnitOfWork unitOfWork, IMapper mapper, IVoucherGroupService voucherGroupService, IMembershipService membershipService) : base(unitOfWork, mapper)
        {
            _voucherGroupService = voucherGroupService;
            _membershipService = membershipService;
        }
        protected override IGenericRepository<Voucher> _repository => _unitOfWork.VoucherRepository;
        protected IGenericRepository<VoucherGroup> _voucherGroupRepos => _unitOfWork.VoucherGroupRepository;
        public async Task<List<Promotion>> CheckVoucher(CustomerOrderInfo order)
        {
            try
            {
                var vouchers = order.Vouchers;
                if (vouchers.Select(el => new { el.VoucherCode, el.PromotionCode }).Distinct().Count() < vouchers.Select(el => new { el.VoucherCode, el.PromotionCode }).Count())
                {
                    throw new ErrorObj(code: 400, message: AppConstant.ErrMessage.Duplicate_VoucherCode, description: AppConstant.ErrMessage.Duplicate_VoucherCode);
                }
                var promotions = new List<Promotion>();
                foreach (var voucherModel in vouchers)
                {
                    var voucher = await _repository.Get(filter: el => el.VoucherCode.Equals(voucherModel.VoucherCode)
                    && el.VoucherGroup.Promotion.PromotionCode.Equals(voucherModel.PromotionCode)
                    && !el.IsUsed,
                    includeProperties:
                    "VoucherGroup.Promotion.PromotionTier.Action.ActionProductMapping.Product," +
                    "VoucherGroup.Promotion.PromotionTier.ConditionRule.ConditionGroup.OrderCondition," +
                    "VoucherGroup.Promotion.PromotionTier.ConditionRule.ConditionGroup.ProductCondition," +
                    "VoucherGroup.Promotion.PromotionStoreMapping.Store");
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
        #region Lấy voucher cho channel
        public async Task<List<Voucher>> GetVouchersForChannel(PromotionChannelMapping voucherChannel, VoucherGroup voucherGroup, VoucherChannelParam channelParam)
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
                    voucher.ChannelId = voucherChannel.ChannelId;
                    _repository.Update(voucher);
                }
                await _unitOfWork.SaveAsync();
            }
            return vouchers.ToList();
        }
        #endregion
        #region Lấy voucher cho customer
        public async Task<VoucherParamResponse> GetVoucherForCustomer(VoucherGroupDto voucherGroupDto)
        {
            try
            {
                if (voucherGroupDto != null)
                {
                    //var entity = _mapper.Map<VoucherGroup>(voucherGroupDto);
                    var vouchers = await _repository.Get(filter: el => el.VoucherGroupId.Equals(voucherGroupDto.VoucherGroupId)
                    && el.IsRedemped == AppConstant.EnvVar.Voucher.UNREDEEM, includeProperties: "VoucherGroup.Promotion");
                    var voucher = vouchers.ToList().First();
                    voucher.IsRedemped = AppConstant.EnvVar.Voucher.REDEEMPED;
                    voucher.RedempedDate = DateTime.Now;
                    _repository.Update(voucher);
                    await _unitOfWork.SaveAsync();
                    //await UpdateVoucherGroupAfterRedeemed(entity);
                    var entity = voucher.VoucherGroup;
                    entity.RedempedQuantity += 1;
                    entity.UpdDate = DateTime.Now;
                    _voucherGroupRepos.Update(entity);
                    await _unitOfWork.SaveAsync();
                    var promoCode = voucher.VoucherGroup.Promotion.PromotionCode;
                    var description = voucher.VoucherGroup.Promotion.Description;
                    return new VoucherParamResponse(voucherGroupId: entity.VoucherGroupId, voucherGroupName: entity.VoucherName,
                        voucherId: voucher.VoucherId, code: promoCode + "-" + voucher.VoucherCode, description: description);
                }
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: null);
            }
            catch (Exception e)
            {

                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e.Message);
            }
        }

        #endregion
        #region Update voucher đã applied
        public async Task<List<Voucher>> UpdateVoucherApplied(CustomerOrderInfo order)
        {

            try
            {
                List<Voucher> result = null;
                if (order != null)
                {
                    var promotions = await CheckVoucher(order);
                    result = new List<Voucher>();
                    foreach (var voucherParam in order.Vouchers)
                    {
                        foreach (var promotion in promotions)
                        {
                            if (voucherParam.PromotionCode.Equals(promotion.PromotionCode))
                            {
                                var voucherGroup = (await _voucherGroupService.
                                    GetAsync(filter: el => el.PromotionId.Equals(promotion.PromotionId), includeProperties: "Voucher")).Data.First();
                                if (voucherGroup.Voucher.Where(el => el.VoucherCode.Equals(voucherParam.VoucherCode)).Distinct().Count()
                                < voucherGroup.Voucher.Where(w => w.VoucherCode.Equals(voucherParam.VoucherCode)).Count())
                                {
                                    throw new ErrorObj(code: 400, message: AppConstant.ErrMessage.Duplicate_VoucherCode,
                                        description: AppConstant.ErrMessage.Duplicate_VoucherCode);
                                }
                                var vouchers = voucherGroup.Voucher.Where(w => w.VoucherCode.Equals(voucherParam.VoucherCode)).First();
                                if (voucherGroup.VoucherType.Equals(AppConstant.EnvVar.VoucherType.STANDALONE_CODE))
                                {
                                    await UpdateVoucherGroupAfterApplied(voucherGroup);
                                }
                                else
                                {
                                    await UpdateVoucherGroupAfterApplied(voucherGroup);
                                    await UpdateVoucherAfterApplied(vouchers, order);
                                }
                                result.Add(vouchers);
                                return result;
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e.Message);
            }

        }
        private async Task UpdateVoucherGroupAfterApplied(VoucherGroup voucherGroup)
        {
            voucherGroup.UpdDate = DateTime.Now;
            voucherGroup.UsedQuantity += 1;
            await _voucherGroupService.UpdateVoucherGroupForApplied(voucherGroup);
        }
        private async Task UpdateVoucherAfterApplied(Voucher voucher, CustomerOrderInfo order)
        {
            voucher.UpdDate = DateTime.Now;
            voucher.UsedDate = DateTime.Now;
            voucher.IsUsed = AppConstant.EnvVar.Voucher.USED;
            if (order.Customer != null)
            {
                var memInfor = order.Customer;
                MembershipDto membership = new MembershipDto();
                membership.MembershipId = Guid.NewGuid();
                membership.InsDate = DateTime.Now;
                membership.Fullname = memInfor.CustomerName;
                membership.Email = memInfor.CustomerEmail;
                membership.PhoneNumber = memInfor.CustomerPhoneNo;
                var result = await _membershipService.CreateAsync(membership);
                voucher.MembershipId = result.MembershipId;
            }
            _repository.Update(voucher);
            await _unitOfWork.SaveAsync();
        }
        #endregion
    }
}

