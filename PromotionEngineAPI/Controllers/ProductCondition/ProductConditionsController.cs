using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;
using ApplicationCore.Services;
using ApplicationCore.Models;
using ApplicationCore.Utils;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
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
        public List<ProductCondition> GetProductCondition()
        {
            return _service.GetProductConditions();
        }

        // GET: api/ProductConditions/5
        [HttpGet("{id}")]
        public ActionResult<ProductCondition> GetProductCondition(Guid id)
        {
            var condition = _service.FindProductCondition(id);

            if (condition == null)
            {
                return NotFound();
            }

            return Ok(condition);
        }

        // PUT: api/ProductConditions/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public ActionResult PutProductCondition(Guid id, ProductConditionParam param)
        {
            if (id != param.ProductConditionId)
            {
                return BadRequest();
            }

            try
            {
                int result = _service.UpdateProductCondition(id, param);
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

        // POST: api/ProductConditions
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult<ProductCondition> PostProductCondition(ProductCondition ProductCondition)
        {
            int result = _service.AddProductCondition(ProductCondition);
            if (result == GlobalVariables.SUCCESS)
            {
                return Ok(ProductCondition);
            }
            else if (result == GlobalVariables.DUPLICATE)
            {
                return Conflict();
            }
            return NoContent();
        }

        // DELETE: api/ProductConditions/5
        [HttpDelete("{id}")]
        public ActionResult<ProductCondition> DeleteProductCondition(Guid id)
        {
            var condition = _service.DeleteProductCondition(id);

            if (condition > 0)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
