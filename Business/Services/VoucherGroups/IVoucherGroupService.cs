using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services.VoucherGroups
{
    public interface IVoucherGroupService
    {
        public List<VoucherGroup> GetVoucherGroups();

        public VoucherGroup GetVoucherGroup(Guid id);

        public int PostVoucherGroup(VoucherGroup voucherGroupMapping);

        public int PutVoucherGroup(VoucherGroup voucherGroupMapping);

        public int DeleteVoucherGroup(Guid id);
    }
}
