using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace Infrastructure.Repository
{
    public interface IVoucherRepository
    {
        public Task DeleteBulk(Guid voucherGroupId);
        public Task InsertBulk(List<Voucher> vouchers);
        public Task<bool> RejectVoucher(Guid voucherGroupId, Guid promotionId);
        public Task<bool> UpdateVoucherGroupWhenDeletetier(Guid voucherGroupId, Guid tierId);
    }
    public class VoucherRepositoryImp : IVoucherRepository
    {
        private const string connectionString = AppConstant.CONNECTION_STRING;  
        public async Task DeleteBulk(Guid voucherGroupId)
        {
            using (var context = new PromotionEngineContext(options: GetDbOption()))
            {
                var vouchers = context.Voucher.Where(x => x.VoucherGroupId.Equals(voucherGroupId));
                await context.BulkDeleteAsync(vouchers);
                var voucherGroup = context.VoucherGroup.FirstOrDefault(x => x.VoucherGroupId.Equals(voucherGroupId));
                await context.SingleDeleteAsync(voucherGroup);
            }
              

        }

        public async Task InsertBulk(List<Voucher> vouchers)
        {
            try
            {
                using (var context = new PromotionEngineContext(options: GetDbOption()))
                {
                    await context.Voucher.AddRangeAsync(vouchers);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error voucher repository: ", ex.Message);
                Debug.WriteLine("Error voucher repository: ",ex.InnerException);
                Debug.WriteLine("Error voucher repository: ", ex.StackTrace);
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: ex.Message);
            }
        }
        private DbContextOptions<PromotionEngineContext> GetDbOption()
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder<PromotionEngineContext>(), connectionString: connectionString).Options;
        }
        public async Task<bool> RejectVoucher(Guid voucherGroupId, Guid promotionId)
        {
            using (var context = new PromotionEngineContext())
            {
                /*  var now = DateTime.Now;
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
                  }*/



                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> UpdateVoucherGroupWhenDeletetier(Guid voucherGroupId, Guid tierId)
        {

            using (var context = new PromotionEngineContext(options: GetDbOption()))
            {
                IQueryable<VoucherGroup> query = context.Set<VoucherGroup>();
                var voucherGroup = await query.Where(el => el.VoucherGroupId.Equals(voucherGroupId)).Include("Voucher").FirstOrDefaultAsync();
                if (voucherGroup != null)
                {
                    var now = DateTime.UtcNow.AddHours(7);
                    context.Entry(voucherGroup).State = EntityState.Modified;
                    var vouchers = voucherGroup.Voucher.Where(el => el.PromotionTierId.Equals(tierId)).ToList();
                    if (vouchers != null && vouchers.Count() > 0)
                    {

                        foreach (var voucher in vouchers)
                        {
                            if (voucher.IsUsed || voucher.IsRedemped)
                            {
                                voucher.PromotionTierId = null;
                            }
                            else
                            {
                                voucher.PromotionTierId = null;
                                voucher.PromotionId = null;
                                voucher.Promotion = null;
                            }
                            voucher.UpdDate = now;
                        }
                        voucherGroup.UpdDate = now;
                    }
                }

                return await context.SaveChangesAsync() > 0;
            }

        }
    }
}
