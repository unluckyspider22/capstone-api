
using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IVoucherGroupService : IBaseService<VoucherGroup, VoucherGroupDto>
    {
        public List<VoucherDto> GenerateBulkCodeVoucher(VoucherGroupDto dto);

        public List<VoucherDto> GenerateStandaloneVoucher(VoucherGroupDto dto);

        public Task<IEnumerable<VoucherParamResponse>> getVoucherForGame(string BrandCode, string StoreCode,int voucherQuantity);
    }
}
