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

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _service;

        public PromotionsController(IPromotionService service)
        {
            _service = service;
        }

        // GET: api/Promotions
        [HttpGet]
        public List<Promotion> GetPromotion()
        {
            return _service.GetPromotions();
        }

        // GET: api/Promotions/5
        [HttpGet("{id}")]
        public ActionResult<Promotion> GetPromotion(Guid id)
        {
            var condition = _service.FindPromotion(id);

            if (condition == null)
            {
                return NotFound();
            }

            return Ok(condition);
        }

        // PUT: api/Promotions/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public ActionResult<Promotion> PutPromotion(Guid id, Promotion param)
        {
            if (id != param.PromotionId)
            {
                return BadRequest();
            }

            try
            {
                int result = _service.UpdatePromotion(id, param);
                if (result == GlobalVariables.SUCCESS)
                {
                    return Ok(param);
                }
                else if (result == GlobalVariables.NOT_FOUND)
                {
                    return NotFound();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
            }

            return NoContent();
        }

        // POST: api/Promotions
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult<Promotion> PostPromotion(Promotion Promotion)
        {
            int result = _service.AddPromotion(Promotion);
            if (result == GlobalVariables.SUCCESS)
            {
                return Ok(Promotion);
            }
            else if (result == GlobalVariables.DUPLICATE)
            {
                return Conflict();
            }
            return NoContent();
        }

        // DELETE: api/Promotions/5
        [HttpDelete("{id}")]
        public ActionResult<Promotion> DeletePromotion(Guid id)
        {
            var condition = _service.DeletePromotion(id);

            if (condition > 0)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
