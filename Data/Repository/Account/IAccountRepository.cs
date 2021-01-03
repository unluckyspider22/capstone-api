using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account> GetByUsername(string username);
    }
}
