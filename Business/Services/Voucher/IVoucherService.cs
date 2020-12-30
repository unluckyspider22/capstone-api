using ApplicationCore.Models.Voucher;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IVoucherService
    {

        public List<Voucher> GetVouchers();

        public Voucher GetVoucher(Guid id);

        public int PostVoucher(Voucher Voucher);

        public int PutVoucher(Guid id, VoucherParam VoucherParam);

        public int DeleteVoucher(Guid id);

        public int CountVoucher();

        public int UpdateDelFlag(Guid id, string delflg);
    }
}
