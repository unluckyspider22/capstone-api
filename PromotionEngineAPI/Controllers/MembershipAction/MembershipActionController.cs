using ApplicationCore.Services;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/membership-actions")]
    [ApiController]
    public class MembershipActionsController : ControllerBase
    {
        private readonly IPostActionService _service;

        public MembershipActionsController(IPostActionService service)
        {
            _service = service;
        }

        // GET: api/MembershipActions
        [HttpGet]
        // api/MembershipActions?pageIndex=...&pageSize=...
        public async Task<IActionResult> GetMembershipAction([FromQuery] PagingRequestParam param)
        {
            var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize, filter: el => !el.DelFlg);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/MembershipActions/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountMembershipAction()
        {
            return Ok(await _service.CountAsync(el => !el.DelFlg));
        }

        // GET: api/MembershipActions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMembershipAction([FromRoute] Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/MembershipActions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMembershipAction([FromRoute] Guid id, [FromBody] MembershipActionDto dto)
        {
            if (id != dto.MembershipActionId)
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

        // POST: api/MembershipActions
        [HttpPost]
        public async Task<IActionResult> PostMembershipAction([FromBody] MembershipActionDto dto)
        {
            dto.MembershipActionId = Guid.NewGuid();

            var result = await _service.CreateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            //var result = dto;

            return Ok(result);
        }

        // DELETE: api/MembershipActions/5
        [HttpDelete]
        public async Task<IActionResult> DeleteMembershipAction([FromQuery] Guid id)
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