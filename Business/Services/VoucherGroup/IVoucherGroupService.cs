using ApplicationCore.Models.VoucherGroup;
using Infrastructure.DTOs.VoucherGroup;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IVoucherGroupService : IBaseService<VoucherGroup, VoucherGroupDto>
    {
      
    }
}
