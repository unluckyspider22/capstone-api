﻿using ApplicationCore.Models.VoucherGroup;
using ApplicationCore.Services;
using ApplicationCore.Utils;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/voucher-groups")]
    [ApiController]
    public class VoucherGroupsController : ControllerBase
    {
        private readonly IVoucherGroupService _service;

        public VoucherGroupsController(IVoucherGroupService service)
        {
            _service = service;
        }

        // GET: api/VoucherGroups
        [HttpGet]
        // api/VoucherGroups?pageIndex=...&pageSize=...
        public async Task<IActionResult> GetVoucherGroup([FromQuery] PagingRequestParam param)
        {
            var result = await _service.GetAsync(pageIndex: param.PageIndex, pageSize: param.PageSize, filter: el => el.DelFlg.Equals("0"));
            if (result == null)
            {

                return StatusCode(statusCode: 500, new ErrorResponse().InternalServerError);
            }
            return Ok(result);
        }

        // GET: api/VoucherGroups/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> CountVoucherGroup()
        {
            return Ok(await _service.CountAsync(el => el.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)));
        }

        // GET: api/VoucherGroups/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVoucherGroup([FromRoute] Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {

                return StatusCode(statusCode: 500, new ErrorResponse().InternalServerError);
            }
            return Ok(result);
        }

        // PUT: api/VoucherGroups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoucherGroup([FromRoute] Guid id, [FromBody] VoucherGroupDto dto)
        {
            if (id != dto.VoucherGroupId)
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }

            dto.UpdDate = DateTime.Now;

            var result = await _service.UpdateAsync(dto);

            if (result == null)
            {

                return StatusCode(statusCode: 500, new ErrorResponse().InternalServerError);
            }

            return Ok(result);

        }

        // POST: api/VoucherGroups
        [HttpPost]
        public async Task<IActionResult> PostVoucherGroup([FromBody] VoucherGroupDto dto)
        {
            dto.VoucherGroupId = Guid.NewGuid();

            var result = await _service.CreateAsync(dto);

            if (result == null)
            {

                return StatusCode(statusCode: 500, new ErrorResponse().InternalServerError);
            }

            //var result = dto;

            return Ok(result);
        }

        // DELETE: api/VoucherGroups/5
        [HttpDelete]
        public async Task<IActionResult> DeleteVoucherGroup([FromQuery] Guid id)
        {
            if (id == null)
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            var result = await _service.DeleteAsync(id);
            if (result == false)
            {

                return StatusCode(statusCode: 500, new ErrorResponse().InternalServerError);
            }
            return Ok();
        }

        
    }
}
