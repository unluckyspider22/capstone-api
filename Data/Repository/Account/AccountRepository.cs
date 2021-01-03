using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(PromotionEngineContext context) : base(context)
        {

        }

        public async Task<Account> GetByUsername(string username)
        {
            return await _dbSet.FindAsync(username);
        }
    }
}
