using ApplicationCore.Request;
using Infrastructure.DTOs;
using Infrastructure.DTOs.Voucher;
using Infrastructure.DTOs.VoucherChannel;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IVoucherService : IBaseService<Voucher, VoucherDto>
    {
        public Task<List<Promotion>> CheckVoucher(CustomerOrderInfo order);

        public Task<List<Voucher>> GetVouchersForChannel(PromotionChannelMapping voucherChannel, VoucherGroup voucherGroup, VoucherChannelParam channelParam);

        public Task<List<Voucher>> UpdateVoucherApplied(Guid transactionId,CustomerOrderInfo order,Guid storeId);

        public Task<VoucherParamResponse> GetVoucherForCustomer(VoucherGroupDto voucherGroupDto);

        public Task<VoucherForCustomerModel> GetVoucherForCusOnSite(VoucherForCustomerModel param, Guid promotionId, Guid tierId);

        public string Encrypt(string Encryptval);

        public string Decrypt(string DecryptText);

        public Task<PromotionVoucherCount> PromoVoucherCount(Guid promotionId, Guid voucherGroupId);

        public Task<CheckVoucherDto> GetCheckVoucherInfo(string searchCode, Guid voucherGroupId);
    }

}
