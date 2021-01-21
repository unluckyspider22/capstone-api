using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IVoucherService : IBaseService<Voucher, VoucherDto>
    {
        public Task<int> activeAllVoucherInGroup(VoucherGroupDto Dto);
    }

}
