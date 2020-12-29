using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;
using ApplicationCore.Services;
using ApplicationCore.Utils;
using ApplicationCore.Models;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderConditionsController : ControllerBase
    {
        private readonly IOrderConditionService _service;

        public OrderConditionsController(IOrderConditionService service)
        {
            _service = service;
        }

        // GET: api/OrderConditions
        [HttpGet]
        public List<OrderCondition> GetOrderCondition()
        {
            return _service.GetOrderConditions();
        }

        // GET: api/OrderConditions/5
        [HttpGet("{id}")]
        public ActionResult<OrderCondition> GetOrderCondition(Guid id)
        {
            var condition = _service.FindOrderCondition(id);

            if (condition == null)
            {
                return NotFound();
            }

            return Ok(condition);
        }

        // PUT: api/OrderConditions/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderCondition(Guid id, OrderConditionParam param)
        {
            if (id != param.OrderConditionId)
            {
                return BadRequest();
            }

            try
            {
                int result = _service.UpdateOrderCondition(id, param);
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

        // POST: api/OrderConditions
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult<OrderCondition> PostOrderCondition(OrderCondition orderCondition)
        {
            int result = _service.AddOrderCondition(orderCondition);
            if (result == GlobalVariables.SUCCESS)
            {
                return Ok(orderCondition);
            }
            else if (result == GlobalVariables.DUPLICATE)
            {
                return Conflict();
            }
            return NoContent();
        }

        // DELETE: api/OrderConditions/5
        [HttpDelete("{id}")]
        public ActionResult<OrderCondition> DeleteOrderCondition(Guid id)
        {
            var condition = _service.DeleteOrderCondition(id);

            if (condition > 0)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
