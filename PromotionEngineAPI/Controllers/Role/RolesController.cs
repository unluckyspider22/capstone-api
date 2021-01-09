using ApplicationCore.Services;
using Infrastructure.DTOs;
using Infrastructure.DTOs.Role;
using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : Controller
    {
        private readonly IRoleService _service;

        public RolesController(IRoleService service)
        {
            _service = service;
        }

        // GET: api/Roles
        [HttpGet]
        // api/Roles?pageIndex=...&pageSize=...
        public async Task<IActionResult> GetRole([FromQuery] PagingRequestParam param)
        {
            var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize, filter: el => el.DelFlg.Equals("0"));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/Roles/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountRole()
        {
            return Ok(await _service.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)));
        }

       


    }
}
