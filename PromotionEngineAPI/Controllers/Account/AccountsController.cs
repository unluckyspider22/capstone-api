using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;
using ApplicationCore.Services;
using ApplicationCore.Utils;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _service;
        public AccountsController(IAccountService service)
        {
            _service = service;
        }

        // GET: api/accounts
        [HttpGet]
        public List<Account> GetAccount()
        {
            return _service.GetAccounts();
        }

        // GET: api/accounts/count
        [HttpGet]
        [Route("count")]
        public int CountAccount()
        {
            return _service.CountAccount();
        }

        // GET: api/accounts/5
        [HttpGet("{username}")]
        public Account GetAccount(string username)
        {
            return _service.GetAccount(username);
        }

        // PUT: api/accounts/5
        [HttpPut("{username}")]
        public ActionResult<Account> PutAccount(string username, Account account)
        {
            if (username != account.Username)
            {
                return BadRequest();
            }

            if (_service.UpdateAccount(username, account) == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // POST: api/accounts
        [HttpPost]
        public ActionResult<Account> PostAccount(Account account)
        {
            if (_service.CreateAccount(account) == GlobalVariables.DUPLICATE)
            {
                return Conflict();
            }

            return Ok(account);
        }

        // DELETE: api/accounts/5
        [HttpDelete("{username}")]
        public ActionResult<Account> DeleteAccount(string username)
        {
            if (_service.DeleteAccount(username) == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }   

            return Ok();
        }

        // PATCH: api/accounts/dev
        [HttpPatch("{username}")]
        public ActionResult<Account> HideAccount(string username)
        {
            if (_service.HideAccount(username) == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }

            return Ok();
        }


    }
}
