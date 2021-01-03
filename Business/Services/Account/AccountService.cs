using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Utils;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services
{
    public class AccountService : BaseService<Account, AccountDto>, IAccountService
    {
        private readonly AccountRepository _accountRepository;
        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, AccountRepository accountRepository) : base(unitOfWork, mapper)
        {
            _accountRepository = accountRepository;
        }

        protected override IGenericRepository<Account> _repository => _unitOfWork.AccountRepositoryImp;




        public async Task<AccountDto> GetByUsernameAsync(string username)
        {
            if (username != null)
            {
                return null;
            }
            var result = await _accountRepository.GetByUsername(username);

            return _mapper.Map<AccountDto>(result);
        }
    }
}
