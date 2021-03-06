using ApplicationCore.Services;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers.ProductCategory
{
    [Route("api/product-category")]
    [ApiController]
    public class ProductCateController : ControllerBase
    {

        private readonly IProductCateService _service;

        public ProductCateController(IProductCateService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductCategory([FromQuery] PagingRequestParam param, [FromQuery] Guid brandId)
        {
            try
            {
                var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize,
                    filter: o => o.BrandId.Equals(brandId) && !o.DelFlg);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }

        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountProductCategory()
        {
            try
            {
                return Ok(await _service.CountAsync());
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductCategory([FromRoute] Guid id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return Ok(result);

            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCategory([FromRoute] Guid id, [FromBody] ProductCategoryDto dto)
        {
            if (id != dto.ProductCateId || id.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _service.Update(dto);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostProductCategory([FromBody] ProductCategoryDto dto)
        {
            if (await _service.CheckExistin(dto.CateId, dto.BrandId))
            {
                return StatusCode(statusCode: 500, new ErrorObj(500, "ProductCategory exist"));
            }
            try
            {
                dto.ProductCateId = Guid.NewGuid();
                dto.InsDate = DateTime.Now;
                dto.UpdDate = DateTime.Now;
                var result = await _service.CreateAsync(dto);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProductCategory([FromQuery] Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _service.DeleteAsync(id);
                return Ok();
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
    }
}
