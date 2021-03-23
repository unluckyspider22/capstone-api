using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IVoucherRepository
    {
        public Task DeleteBulk(Guid voucherGroupId);
        public Task InsertBulk(List<Voucher> vouchers);

    }
    public class VoucherRepositoryImp : IVoucherRepository
    {
        private PromotionEngineContext context = new PromotionEngineContext();
        public async Task DeleteBulk(Guid voucherGroupId)
        {
            var vouchers = context.Voucher.Where(x => x.VoucherGroupId.Equals(voucherGroupId));
            await context.BulkDeleteAsync(vouchers);
            var voucherGroup = context.VoucherGroup.FirstOrDefault(x => x.VoucherGroupId.Equals(voucherGroupId));
            await context.SingleDeleteAsync(voucherGroup);

        }

        public async Task InsertBulk(List<Voucher> vouchers)
        {
            using (var context = new PromotionEngineContext())
            {
                await context.BulkInsertAsync(vouchers, b => b.IncludeGraph = true);
            }

        }
    }
}
