
using Infrastructure.DTOs;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IAccountService : IBaseService<Account, AccountDto>
    {
        Task<AccountDto> GetByUsernameAsync(string username);
    }
}
