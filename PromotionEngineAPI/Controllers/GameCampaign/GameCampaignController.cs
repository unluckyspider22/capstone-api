using ApplicationCore.Services;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/game-campaign")]
    [ApiController]
    public class GameCampaignController : ControllerBase
    {
        private readonly IGameCampaignService _service;

        public GameCampaignController(IGameCampaignService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetGameConfig([FromQuery] PagingRequestParam param, [FromQuery] Guid brandId)
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
        public async Task<IActionResult> UpdateGame([FromBody] GameConfigDto dto, [FromQuery] Guid gameConfigId)
        {
            if (dto.BrandId.Equals(Guid.Empty) || !dto.Id.Equals(gameConfigId) || gameConfigId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _service.UpdateGameConfig(dto);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] GameConfigDto dto)
        {
            if (dto.BrandId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }

            try
            {
                dto.UpdDate = DateTime.Now;
                dto.InsDate = DateTime.Now;
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
        public async Task<IActionResult> DeleteGameConfig([FromRoute] Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _service.DeleteGameConfig(id);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpGet]
        [Route("info")]
        public async Task<IActionResult> GetGameConfigInfo([FromQuery] Guid gameConfigId, [FromQuery] Guid brandId)
        {
            if (gameConfigId.Equals(Guid.Empty) || brandId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _service.GetFirst(filter: o => o.Id.Equals(gameConfigId) && o.BrandId.Equals(brandId) && !o.DelFlg,
                    includeProperties: "GameItems");
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
        [HttpGet]
        [Route("device/{deviceId}/game-campaign/{gameCode}")]
        public async Task<IActionResult> GetGameCampaignDevice([FromRoute] Guid deviceId, [FromRoute]string gameCode)
        {
            if (deviceId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _service.GetGameCampaignItems(deviceId, gameCode);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
    }
}
