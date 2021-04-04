using ApplicationCore.Services;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/actions")]
    [ApiController]
    public class ActionsController : ControllerBase
    {
        private readonly IActionService _service;

        public ActionsController(IActionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAction(
            [FromQuery] PagingRequestParam param,
            [FromQuery] Guid brandId,
            [FromQuery] int ActionType = 0)
        {
            try
            {
                if (brandId.Equals(Guid.Empty))
                {
                    return StatusCode(statusCode: 400, new ErrorObj(400, "Required Brand Id"));
                }
                Expression<Func<Infrastructure.Models.Action, bool>> myFilter = el => !el.DelFlg && el.BrandId.Equals(brandId);
                if (ActionType > 0)
                {
                    myFilter = el => !el.DelFlg 
                                     && el.BrandId.Equals(brandId) 
                                     && el.ActionType == ActionType;
                }
                var result = await _service.GetAsync(
                pageIndex: param.PageIndex,
                pageSize: param.PageSize,
                filter: myFilter,
                orderBy: el => el.OrderByDescending(b => b.InsDate));
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAction([FromRoute] Guid id)
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
        [HttpPut("{actionId}")]
        public async Task<IActionResult> PutAction([FromRoute] Guid actionId, [FromBody] ActionDto dto)
        {
            if (!actionId.Equals(dto.ActionId) || dto.ActionId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            var exist = (await _service.GetFirst(filter: o => o.ActionId.Equals(actionId) && !o.DelFlg));
            if (exist == null)
            {
                return StatusCode(statusCode: 400, new ErrorObj(400, "Action not exist"));
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
        public async Task<IActionResult> PostAction([FromBody] ActionDto dto)
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
        [Route("{actionId}")]
        public async Task<IActionResult> DeleteAction([FromRoute] Guid actionId)
        {
            if (actionId.Equals(Guid.Empty))
            {
                return StatusCode(statusCode: 400, new ErrorResponse().BadRequest);
            }
            try
            {
                var result = await _service.DeleteAsync(actionId);
                return Ok(result);
            }
            catch (ErrorObj e)
            {
                return StatusCode(statusCode: e.Code, e);
            }
        }
    }
}