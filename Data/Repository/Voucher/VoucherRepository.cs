using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace Infrastructure.Repository.Voucher
{
    public interface IVoucherRepository
    {
        public void DeleteBulk(Guid voucherGroupId);
        public Task<List<Models.Voucher>> InsertBulk(List<Models.Voucher> vouchers);
    }
    public class VoucherRepositoryImp : IVoucherRepository
    {
        private PromotionEngineContext ctx = new PromotionEngineContext();
        public void DeleteBulk(Guid voucherGroupId)
        {
            ctx.Voucher.Where(x => x.VoucherGroupId.Equals(voucherGroupId)).Delete();
        }

        public async Task<List<Models.Voucher>> InsertBulk(List<Models.Voucher> vouchers)
        {
            await using (var transaction = ctx.Database.BeginTransaction())
            {
                ctx.BulkInsert(vouchers);
                transaction.Commit();
            }
            return vouchers;
        }
    }
}
