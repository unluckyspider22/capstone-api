
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace ApplicationCore.Services
{
    public interface IAccountService
    {
        public List<Account> GetAccounts();
        public Account GetAccount(string username);
        public int CreateAccount(Account account);
        public int UpdateAccount(string username, Account account);
        public int DeleteAccount(string username);
        public int CountAccount();
    }
}
