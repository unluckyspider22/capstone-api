using ApplicationCore.Services;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/post-actions")]
    [ApiController]
    public class PostActionsController : ControllerBase
    {
        private readonly IPostActionService _service;

        public PostActionsController(IPostActionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetPostAction([FromQuery] PagingRequestParam param, [FromQuery] Guid brandId)
        {
            try
            {
                if (brandId.Equals(Guid.Empty))
                {
                    return StatusCode(statusCode: 400, new ErrorObj(400, "Required Brand Id"));
                }
                return Ok(await _service.GetAsync(
                pageIndex: param.PageIndex,
                pageSize: param.PageSize,
                filter: el => !el.DelFlg && el.BrandId.Equals(brandId),
                orderBy: el => el.OrderByDescending(b => b.InsDate)));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostAction([FromRoute] Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                return Ok(await _service.GetByIdAsync(id));
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }
        [HttpPut("{postActionId}")]
        public async Task<IActionResult> PutPostAction([FromRoute] Guid postActionId, [FromBody] PostActionDto dto)
        {
            if (!postActionId.Equals(dto.PostActionId) || dto.PostActionId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            var exist = (await _service.GetFirst(filter: o => o.PostActionId.Equals(postActionId) && !o.DelFlg));
            if (exist == null)
            {
                return StatusCode(statusCode: 400, new ErrorObj(400, "Post Action not exist"));
            }
            try
            {
                dto.UpdDate = DateTime.Now;
                dto.InsDate = exist.InsDate;
                var result = await _service.UpdateAsync(dto);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostAction([FromBody] PostActionDto dto)
        {

            if (dto.BrandId.Equals(Guid.Empty))
            {
                return StatusCode(400, new ErrorObj(400, "Required Brand Id"));
            }
            try
            {
                var result = await _service.MyAddAction(dto);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }

        [HttpDelete]
        [Route("{postActionId}")]
        public async Task<IActionResult> DeletePostAction([FromRoute] Guid postActionId)
        {
            if (postActionId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _service.DeleteAsync(postActionId);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }


    }
}