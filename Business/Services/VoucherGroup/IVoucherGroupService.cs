
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
        public List<VoucherDto> GenerateVoucher(decimal? quantity, string charset, string prefix, string postFix, int codeLength = 10, string customCharset = "");
    }
}
