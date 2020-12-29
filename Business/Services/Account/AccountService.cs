using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Utils;
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
                    return GlobalVariables.DUPLICATE;
                }
                else
                {
                    throw;
                }
            }

            return GlobalVariables.SUCCESS;
        }

        public int DeleteAccount(string username)
        {
            var account = _context.Account.Find(username);
            if (account == null)
            {
                return GlobalVariables.NOT_FOUND;
            }

            account.DelFlg = GlobalVariables.DELETED;

            try
            {
                _context.Entry(account).Property("DelFlg").IsModified = true;
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }


            return GlobalVariables.SUCCESS;
        }

        public Account GetAccount(string username)
        {
            var account = _context.Account.Where(c => c.DelFlg.Equals("0")).Where(c => c.Username.Equals(username)).First();

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
                    return GlobalVariables.NOT_FOUND;
                } else
                {
                    throw;
                }
            }

            return GlobalVariables.SUCCESS;
        }

        private bool AccountExists(string id)
        {
            return _context.Account.Any(e => e.Username == id);
        }
    }
}
