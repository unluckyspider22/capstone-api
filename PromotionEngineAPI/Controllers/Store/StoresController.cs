using ApplicationCore.Models;
using ApplicationCore.Models.Store;
using ApplicationCore.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Utils;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreService _service;

        public StoresController(IStoreService service)
        {
            _service = service;
        }

        // GET: api/Stores
        [HttpGet]
        // api/Stores?pageIndex=...&pageSize=...
        public async Task<IActionResult> GetStore([FromQuery] PagingRequestParam param)
        {
            var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize, filter: el => el.DelFlg.Equals("0"));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/Stores/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountStore()
        {
            return Ok(await _service.CountAsync());
        }

        // GET: api/Stores/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStore([FromRoute] Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/Stores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStore([FromRoute] Guid id, [FromBody] StoreDto dto)
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

        // POST: api/Stores
        [HttpPost]
        public async Task<IActionResult> PostStore([FromBody] StoreDto dto)
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

        // DELETE: api/Stores/5
        [HttpDelete]
        public async Task<IActionResult> DeleteStore([FromQuery] Guid id)
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

        // Put: api/Stores/5
        [HttpPatch]
        public async Task<IActionResult> HideStore([FromQuery] Guid id, [FromQuery] string value)
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

