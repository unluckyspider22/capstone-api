using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Services;
using ApplicationCore.Utils;
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
        public List<Holiday> GetHoliday()
        {
            return _service.GetHoliday();
        }

        // GET: api/Holidays/count
        [HttpGet]
        [Route("count")]
        public int CountHoliday()
        {
            return _service.CountHoliday();
        }

        // GET: api/Holidays/5
        [HttpGet("{id}")]
        public Holiday GetHoliday(Guid id)
        {
            return _service.GetHolidays(id);
        }

        // PUT: api/Holidays/5
        [HttpPut("{id}")]
        public ActionResult<Holiday> PutHoliday(Guid id, Holiday holiday)
        {

            if (!id.Equals(holiday.HolidayId))
            {
                return BadRequest();
            }

            var result = _service.UpdateHoliday(id, holiday);

            if (result == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }

            return Ok(holiday);
        }

        // POST: api/Holidays
        [HttpPost]
        public ActionResult<Holiday> PostHoliday(Holiday holiday)
        {
            holiday.HolidayId = Guid.NewGuid();

            _service.CreateHoliday(holiday);

            return Ok(holiday);
        }

        // DELETE: api/Holidays/5
        [HttpDelete("{id}")]
        public ActionResult DeleteHoliday(Guid id)
        {
            var result = _service.DeleteHoliday(id);
            if (result == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }
            return Ok();
        }


        // PATCH: api/Holidays/5
        [HttpPatch("{id}")]
        public ActionResult HideHoliday(Guid id)
        {
            var result = _service.HideHoliday(id);
            if (result == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}