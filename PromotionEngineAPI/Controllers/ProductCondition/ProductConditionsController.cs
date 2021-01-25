using System;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Services;
using System.Threading.Tasks;
using Infrastructure.DTOs;
using Infrastructure.Helper;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/product-conditions")]
    [ApiController]
    public class ProductConditionsController : ControllerBase
    {
        private readonly IProductConditionService _service;

        public ProductConditionsController(IProductConditionService service)
        {
            _service = service;
        }

        // GET: api/ProductConditions
        [HttpGet]
        // api/ProductConditions?pageIndex=...&pageSize=...
        public async Task<IActionResult> GetProductCondition([FromQuery] PagingRequestParam param)
        {
            var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize, filter: el => !el.DelFlg);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // GET: api/ProductConditions/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountProductCondition()
        {
            return Ok(await _service.CountAsync(el => !el.DelFlg));
        }

        // GET: api/ProductConditions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductCondition([FromRoute]Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/ProductConditions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCondition([FromRoute]Guid id, [FromBody] ProductConditionDto dto)
        {
            if (id != dto.ProductConditionId)
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

        // POST: api/ProductConditions
        [HttpPost]
        public async Task<IActionResult> PostProductCondition([FromBody] ProductConditionDto dto)
        {
            dto.ProductConditionId = Guid.NewGuid();

            var result = await _service.CreateAsync(dto);

            if (result == null)
            {
                return NotFound();
            }

            //var result = dto;

            return Ok(result);
        }

        // DELETE: api/ProductConditions/5
        [HttpDelete]
        public async Task<IActionResult> DeleteProductCondition([FromQuery]Guid id)
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
