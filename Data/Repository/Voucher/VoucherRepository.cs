using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
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
        public Task<bool> RejectVoucher(Guid voucherGroupId, Guid promotionId);
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

        public async Task<bool> RejectVoucher(Guid voucherGroupId, Guid promotionId)
        {
            using (var context = new PromotionEngineContext())
            {
                var now = DateTime.Now;
                var voucher = await context.VoucherGroup.FindAsync(voucherGroupId);
                var promo = await context.Promotion.FindAsync(promotionId);
                if (voucher != null)
                {
                    context.Entry(voucher).State = EntityState.Modified;
                    voucher.Promotion = null;
                    voucher.PromotionId = null;
                    voucher.UpdDate = now;
                    context.VoucherGroup.Update(voucher);
                }
                if (promo != null)
                {
                    context.Entry(promo).State = EntityState.Modified;
                    promo.VoucherGroup = null;
                    promo.UpdDate = now;
                }



                return await context.SaveChangesAsync() > 0;
            }
        }
    }
}
