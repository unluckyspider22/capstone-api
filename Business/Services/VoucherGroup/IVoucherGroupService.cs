using ApplicationCore.Models.VoucherGroup;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IVoucherGroupService
    {
        public List<VoucherGroup> GetVoucherGroups();

        public VoucherGroupParam GetVoucherGroup(Guid id);

        public int PostVoucherGroup(VoucherGroup voucherGroup);

        public int PutVoucherGroup(Guid id, VoucherGroupParam voucherGroupParam);

        public int DeleteVoucherGroup(Guid id);

        public int CountVoucherGroup();

        public int UpdateDelFlag(Guid id, string delflg);
    }
}
