using ApplicationCore.Services;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _service;

        public GameController(IGameService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetGame([FromQuery] PagingRequestParam param, [FromQuery] Guid brandId)
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

        [HttpPut]
        public async Task<IActionResult> UpdateGame([FromBody] GameDto dto)
        {
            if (dto.BrandId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _service.UpdateAsync(dto);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] GameDto dto)
        {
            if (dto.BrandId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }

            try
            {
                var result = await _service.CreateAsync(dto);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteGame([FromRoute] Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _service.DeleteAsync(id);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
    }
}
