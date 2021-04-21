﻿using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IPromotionRepository
    {
        public Task<bool> SetUnlimitedDate(Promotion entity);
    }
    public class PromotionRepositoryImp : IPromotionRepository
    {
        private const string connectionString = AppConstant.CONNECTION_STRING;
        public async Task<bool> SetUnlimitedDate(Promotion entity)
        {
            using (var context = new PromotionEngineContext(options: GetDbOption()))
            {
                context.Promotion.Attach(entity);
                context.Entry(entity).Property(e => e.EndDate).IsModified = true;
                return await context.SaveChangesAsync() > 0;
            }
        }

        private DbContextOptions<PromotionEngineContext> GetDbOption()
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder<PromotionEngineContext>(), connectionString: connectionString).Options;
        }
    }
}
