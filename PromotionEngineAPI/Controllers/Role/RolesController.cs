using ApplicationCore.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
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
            return Ok(await _service.CountAsync());
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole([FromRoute] Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole([FromRoute] Guid id, [FromBody] RoleDto dto)
        {
            if (id != dto.Id)
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

        // POST: api/Roles
        [HttpPost]
        public async Task<IActionResult> PostRole([FromBody] RoleDto dto)
        {
            dto.Id = Guid.NewGuid();

            var result = await _service.CreateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            //var result = dto;

            return Ok(result);
        }

        // DELETE: api/Roles/5
        [HttpDelete]
        public async Task<IActionResult> DeleteRole([FromQuery] Guid id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var result = await _service.DeleteAsync(id);
            if (result == false)
            {
                return NotFound();
            }
            return Ok();
        }

        // Put: api/Roles/5
        [HttpPatch]
        public async Task<IActionResult> HideRole([FromQuery] Guid id, [FromQuery] string value)
        {
            if (id == null)
            {
                return BadRequest();
            }
            if (!value.Equals(AppConstant.DelFlg.HIDE) && !value.Equals(AppConstant.DelFlg.UNHIDE))
            {
                return BadRequest();
            }
            var result = await _service.HideAsync(id, value);
            if (result == false)
            {
                return NotFound();
            }
            return Ok();
        }


    }
}
