using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IPromotionRepository
    {
        public Task<bool> SetUnlimitedDate(Promotion entity);
    }
    public class PromotionRepositoryImp : IPromotionRepository
    {
        private PromotionEngineContext ctx = new PromotionEngineContext();
        public async Task<bool> SetUnlimitedDate(Promotion entity)
        {
            ctx.Promotion.Attach(entity);
            ctx.Entry(entity).Property(e => e.EndDate).IsModified = true;
            return await ctx.SaveChangesAsync() > 0;
        }
    }
}
