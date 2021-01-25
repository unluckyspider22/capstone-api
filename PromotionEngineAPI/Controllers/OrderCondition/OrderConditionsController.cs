using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Services;
using Infrastructure.DTOs;
using Infrastructure.Helper;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/order-conditions")]
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
        // api/OrderConditions?pageIndex=...&pageSize=...
        public async Task<IActionResult> GetOrderCondition([FromQuery] PagingRequestParam param)
        {
            var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize, filter: el => !el.DelFlg);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/OrderConditions/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountOrderCondition()
        {
            return Ok(await _service.CountAsync(el => !el.DelFlg));
        }

        // GET: api/OrderConditions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderCondition([FromRoute]Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/OrderConditions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderCondition([FromRoute]Guid id, [FromBody] OrderConditionDto dto)
        {
            if (id != dto.OrderConditionId)
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

        // POST: api/OrderConditions
        [HttpPost]
        public async Task<IActionResult> PostOrderCondition([FromBody] OrderConditionDto dto)
        {
            dto.OrderConditionId = Guid.NewGuid();

            var result = await _service.CreateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            //var result = dto;

            return Ok(result);
        }

        // DELETE: api/OrderConditions/5
        [HttpDelete]
        public async Task<IActionResult> DeleteOrderCondition([FromQuery]Guid id)
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
