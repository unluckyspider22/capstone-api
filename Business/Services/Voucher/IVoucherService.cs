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
        //public Task<int> ActiveAllVoucherInGroup(VoucherGroupDto Dto);

        public Task<List<Promotion>> CheckVoucher(OrderResponseModel order);

        public Task<List<Voucher>> GetVouchersForChannel(PromotionChannelMapping voucherChannel, VoucherGroup voucherGroup, VoucherChannelParam channelParam);

        public Task<List<Voucher>> UpdateVoucherApplied(OrderResponseModel order);

        public Task UpdateVoucherRedeemed(Voucher voucher);
    }

}
