﻿using ApplicationCore.Services;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/devices")]
    [ApiController]
    public class DeviceController : ControllerBase
    {

        private readonly IDeviceService _service;

        public DeviceController(IDeviceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetDevice([FromQuery] PagingRequestParam param, [FromQuery] Guid storeId)
        {
            try
            {
                var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize,
                    filter: o => o.StoreId.Equals(storeId) && !o.DelFlg);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }
        [HttpGet]
        [Route("game-item")]
        public async Task<IActionResult> GetGameItem(string deviceCode, string brandCode)
        {
            try
            {
                var result = await _service.GetFirst(filter: el =>
                            !el.DelFlg
                            && el.Store.Brand.BrandCode == brandCode
                            && el.Code != deviceCode,
                            includeProperties:
                            "Store.Brand," +
                            "GameCampaign.GameItems");

                var campaign = result.GameCampaign;
                if(campaign != null)
                {
                    return Ok(campaign.GameItems);
                }
                return StatusCode(statusCode: (int)HttpStatusCode.NotFound);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountDevice()
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
        [HttpGet]
        [Route("brand")]
        public async Task<IActionResult> GetBrandDevice([FromQuery] PagingRequestParam param, [FromQuery] Guid brandId)
        {
            try
            {
                var result = await _service.GetBrandDevice(PageSize: param.PageSize, PageIndex: param.PageIndex, brandId: brandId);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDevice([FromRoute] Guid id)
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
        public async Task<IActionResult> PutDevice([FromRoute] Guid id, [FromBody] DeviceDto dto)
        {
            if (id != dto.DeviceId || id.Equals(Guid.Empty))
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
        public async Task<IActionResult> PostDevice([FromBody] DeviceDto dto)
        {
            if (dto.GameCampaignId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorObj(400, "Select Game Configuration"));
            }
            try
            {
                dto.DeviceId = Guid.NewGuid();
                dto.Code = _service.GenerateCode(dto.DeviceId);
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
        [Route("pair/{pairCode}")]
        public async Task<IActionResult> PairDevice([FromRoute] string pairCode)
        {

            try
            {
                var result = await _service.GetTokenDevice(pairCode);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDevice([FromQuery] Guid id)
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
