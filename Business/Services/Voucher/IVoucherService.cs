using ApplicationCore.Request;
using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IVoucherService : IBaseService<Voucher, VoucherDto>
    {
        public Task<int> activeAllVoucherInGroup(VoucherGroupDto Dto);

        public Task<List<Promotion>> CheckVoucher(OrderResponseModel order);
    }

}
