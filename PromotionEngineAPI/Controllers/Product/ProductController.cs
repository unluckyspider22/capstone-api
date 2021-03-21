using ApplicationCore.Services;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers.Product
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct([FromQuery] PagingRequestParam param, [FromQuery] Guid productCateId)
        {
            try
            {
                var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize,
                    filter: o => o.ProductCateId.Equals(productCateId) && !o.DelFlg);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }

        [HttpGet]
        [Route("brand")]
        public async Task<IActionResult> GetBrandProduct([FromQuery] PagingRequestParam param, [FromQuery] Guid brandId)
        {
            try
            {
                var result = await _service.GetBrandProduct(PageIndex: param.PageIndex, PageSize: param.PageSize, brandId: brandId);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }

        [HttpGet]
        [Route("brand/all/{brandId}")]
        public async Task<IActionResult> GetAllBrandProduct([FromRoute] Guid brandId)
        {
            try
            {
                var result = await _service.GetAllBrandProduct(brandId: brandId);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }

        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountProduct()
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
        public async Task<IActionResult> GetProduct([FromRoute] Guid id)
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
        public async Task<IActionResult> PutProduct([FromRoute] Guid id, [FromBody] ProductDto dto)
        {
            if (id != dto.ProductId || id.Equals(Guid.Empty))
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
        public async Task<IActionResult> PostProduct([FromBody] ProductDto dto)
        {
            if (await _service.CheckExistin(cateId: dto.CateId, code: dto.Code, productCateId: dto.ProductCateId))
            {
                return StatusCode(statusCode: 500, new ErrorObj(500, "Product exist"));
            }
            try
            {
                dto.ProductId = Guid.NewGuid();
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
        [HttpPost]
        [Route("sync-product")]
        public async Task<IActionResult> SyncProduct([FromQuery] Guid brandId, [FromBody]  ProductRequestParam productRequestParam)
        {

            try
            {
                var result = await _service.SyncProduct(brandId, productRequestParam);
                return Ok(result);
            }
            
            catch (ErrorObj e)
            {

                return StatusCode(statusCode: e.Code, e);
            }   
            
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct([FromQuery] Guid id)
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
