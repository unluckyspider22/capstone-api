using System;
using System.Threading.Tasks;
using ApplicationCore.Services;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipActionsController : ControllerBase
    {
        private readonly IMembershipActionService _service;

        public MembershipActionsController(IMembershipActionService service)
        {
            _service = service;
        }

        // GET: api/MembershipActions
        [HttpGet]
        // api/MembershipActions?pageIndex=...&pageSize=...
        public async Task<IActionResult> GetMembershipAction([FromQuery] PagingRequestParam param)
        {
            var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize, filter: el => el.DelFlg.Equals("0"));
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
            return Ok(await _service.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)));
        }

        // GET: api/MembershipActions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMembershipAction([FromRoute]Guid id)
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
        public async Task<IActionResult> PutMembershipAction([FromRoute]Guid id, [FromBody] MembershipActionDto dto)
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
        public async Task<IActionResult> DeleteMembershipAction([FromQuery]Guid id)
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

        // Put: api/MembershipActions/5
        [HttpPatch]
        public async Task<IActionResult> HideMembershipAction([FromQuery]Guid id, [FromQuery]string value)
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