using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Services;
using ApplicationCore.Utils;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidayService _service;

        public HolidaysController(IHolidayService service)
        {
            _service = service;
        }

        // GET: api/Holidays
        [HttpGet]
        public async Task<IActionResult> GetHoliday([FromQuery] PagingRequestParam param)
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

        // GET: api/Holidays/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountHoliday()
        {
            return Ok(await _service.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)));
        }

        // GET: api/Holidays/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHoliday([FromRoute]Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/Holidays/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHoliday([FromRoute]Guid id, [FromBody] HolidayDto dto)
        {
            if (id != dto.HolidayId)
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

        // POST: api/Holidays
        [HttpPost]
        public async Task<IActionResult> PostHoliday([FromBody] HolidayDto dto)
        {
            dto.HolidayId = Guid.NewGuid();

            var result = await _service.CreateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            //var result = dto;

            return Ok(result);
        }

        // DELETE: api/Holidays/5
        [HttpDelete]
        public async Task<IActionResult> DeleteHoliday([FromQuery]Guid id)
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