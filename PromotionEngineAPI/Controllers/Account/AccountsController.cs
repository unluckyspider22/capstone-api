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
using Infrastructure.DTOs;
using Infrastructure.Helper;

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

        // GET: api/Accounts
        [HttpGet]
        public async Task<IActionResult> GetAccount([FromQuery] PagingRequestParam param)
        {
            var result = await _service.GetAsync(
                pageIndex: param.PageIndex,
                pageSize: param.PageSize,
                filter: el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE),
                orderBy: el => el.OrderByDescending(b => b.InsDate)
                );

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/Accounts/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountAccount()
        {
            return Ok(await _service.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)));
        }

        //GET: api/Accounts/5
        [HttpGet("{username}")]
        public async Task<IActionResult> GetAccount([FromRoute]string username)
        {
            var result = await _service.GetByUsernameAsync(username);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/Accounts/5
        [HttpPut("{username}")]
        public async Task<IActionResult> PutAccount([FromRoute]string username, [FromBody] AccountDto dto)
        {
            if (!username.Equals(dto.Username))
            {
                return BadRequest();
            }

            dto.UpdDate = DateTime.Now;

            var result = await _service.UpdateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);

        }

        // POST: api/Accounts
        [HttpPost]
        public async Task<IActionResult> PostAccount([FromBody] AccountDto dto)
        {
            var result = await _service.CreateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // DELETE: api/Accounts/5
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount([FromQuery]string username)
        {
            if (username == null)
            {
                return BadRequest();
            }
            var result = await _service.DeleteUsernameAsync(username);
            if (result == false)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
