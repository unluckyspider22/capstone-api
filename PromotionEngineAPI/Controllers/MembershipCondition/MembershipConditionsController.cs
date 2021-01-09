using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;
using ApplicationCore.Services;
using ApplicationCore.Models;
using ApplicationCore.Utils;
using Infrastructure.DTOs;
using Infrastructure.Helper;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/membership-conditions")]
    [ApiController]
    public class MembershipConditionsController : ControllerBase
    {
        private readonly IMembershipConditionService _service;

        public MembershipConditionsController(IMembershipConditionService service)
        {
            _service = service;
        }

        // GET: api/MembershipConditions
        [HttpGet]
        // api/MembershipConditions?pageIndex=...&pageSize=...
        public async Task<IActionResult> GetMembershipCondition([FromQuery] PagingRequestParam param)
        {
            var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize, filter: el => el.DelFlg.Equals("0"));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/MembershipConditions/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountMembershipCondition()
        {
            return Ok(await _service.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)));
        }

        // GET: api/MembershipConditions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMembershipCondition([FromRoute]Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/MembershipConditions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMembershipCondition([FromRoute]Guid id, [FromBody] MembershipConditionDto dto)
        {
            if (id != dto.MembershipConditionId)
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

        // POST: api/MembershipConditions
        [HttpPost]
        public async Task<IActionResult> PostMembershipCondition([FromBody] MembershipConditionDto dto)
        {
            dto.MembershipConditionId = Guid.NewGuid();

            var result = await _service.CreateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            //var result = dto;

            return Ok(result);
        }

        // DELETE: api/MembershipConditions/5
        [HttpDelete]
        public async Task<IActionResult> DeleteMembershipCondition([FromQuery]Guid id)
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
