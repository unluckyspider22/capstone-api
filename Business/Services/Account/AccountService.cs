using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Utils;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services
{
    public class AccountService : BaseService<Account, AccountDto>, IAccountService
    {
        public AccountService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }
        protected override IGenericRepository<Account> _repository => _unitOfWork.AccountRepository;

        public async Task<bool> DeleteUsernameAsync(string username)
        {
            _repository.DeleteUsername(username);
            return await _unitOfWork.SaveAsync() > 0;
        }

        public async Task<Account> GetByUsernameAsync(string username)
        {
            var result = await _repository.GetFirst(
                el => el.Username.Equals(username)
                && el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE), 
                includeProperties: "Brand"
                );

            return result;
        }

        public async Task<bool> HideUsernameAsync(string username, string value)
        {
            _repository.HideUsername(username, value);
            return await _unitOfWork.SaveAsync() > 0;
        }
    }
}
