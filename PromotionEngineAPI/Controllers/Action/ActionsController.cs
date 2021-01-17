using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ApplicationCore.Services;

using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/actions")]
    [ApiController]
    public class ActionsController : ControllerBase
    {
        private readonly IActionService _service;

        public ActionsController(IActionService service)
        {
            _service = service;
        }

        // GET: api/Actions
        [HttpGet]
        public async Task<IActionResult> GetAction([FromQuery] PagingRequestParam param)
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

        // GET: api/Actions/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountAction()
        {
            return Ok(await _service.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)));
        }

        // GET: api/Actions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAction([FromRoute]System.Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/Actions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAction([FromRoute]System.Guid id, [FromBody] ActionDto dto)
        {
            if (id != dto.ActionId)
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

        // POST: api/Actions
        [HttpPost]
        public async Task<IActionResult> PostAction([FromBody] ActionDto dto)
        {
            dto.ActionId = Guid.NewGuid();

            var result = await _service.CreateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            //var result = dto;

            return Ok(result);
        }

        // DELETE: api/Actions/5
        [HttpDelete]
        public async Task<IActionResult> DeleteAction([FromQuery]Guid id)
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

        
    }
}