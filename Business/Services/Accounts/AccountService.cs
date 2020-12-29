using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services
{
    public class AccountService : IAccountService
    {
        private readonly PromotionEngineContext _context;
        public AccountService(PromotionEngineContext context)
        {
            _context = context;
        }

        public int CountAccount()
        {
            int count = _context.Account.Where(c => c.DelFlg.Equals("0")).Count();
            return count;
        }

        public int CreateAccount(Account account)
        {
            _context.Account.Add(account);
            try
            {
                 _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AccountExists(account.Username))
                {
                    return 0;
                }
                else
                {
                    throw;
                }
            }

            return 1;
        }

        public int DeleteAccount(string username)
        {
            var account =  _context.Account.Find(username);
            if (account == null)
            {
                return 0;
            }
            account.DelFlg = "1";
            _context.Entry(account).Property("DelFlg").IsModified = true;
            _context.SaveChanges();

            return 1;
        }

        public Account GetAccount(string username)
        {
            var account = _context.Account.Where(c => c.DelFlg.Equals("0")).Where(c => c.Username.Equals(username)).FirstOrDefault() ;

            return account;
        }

        public List<Account> GetAccounts()
        {
            var result = _context.Account.Where(c => c.DelFlg.Equals("0")).ToList();
            return result;
        }

        public int UpdateAccount(string username, Account account)
        {

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(username))
                {
                    return 0;
                }
            }

            return 1;
        }

        private bool AccountExists(string id)
        {
            return _context.Account.Any(e => e.Username == id);
        }
    }
}
