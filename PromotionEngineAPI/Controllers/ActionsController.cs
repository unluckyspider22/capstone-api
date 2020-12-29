using System.Collections.Generic;
using ApplicationCore.Models;
using ApplicationCore.Services.Actions;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionsController : ControllerBase
    {
        private readonly IActionService _service;

        public ActionsController(IActionService service)
        {
            _service = service;
        }

        // GET: api/actions
        [HttpGet]
        public List<Action> GetAction()
        {
            return _service.GetActions();
        }

        // GET: api/Actions/count
        [HttpGet]
        [Route("count")]
        public int CountAction()
        {
            return _service.CountAction();
        }

        // GET: api/Actions/5
        [HttpGet("{id}")]
        public Action GetAction(string id)
        {
            System.Guid param = new System.Guid(id);

            return _service.GetActions(param);
        }

        // PUT: api/Actions/5
        [HttpPut("{id}")]
        public ActionResult<Action> PutAction(string id, ActionParam actionParam)
        {
            System.Guid param = new System.Guid(id);

            if (!param.Equals(actionParam.ActionId))
            {
                return BadRequest();
            }

            if (_service.UpdateAction(param, actionParam) == 0)
            {
                return NotFound();
            }

            return Ok(actionParam);
        }

        // POST: api/Actions
        [HttpPost]
        public ActionResult<Action> PostAction(ActionParam actionParam)
        {
            actionParam.ActionId = System.Guid.NewGuid();

            if (_service.CreateAction(actionParam) == 0)
            {
                return Conflict();
            }
            return Ok(actionParam);
        }

        // DELETE: api/Actions/5
        [HttpDelete("{id}")]
        public ActionResult<Action> DeleteAction(string id)
        {
            System.Guid param = new System.Guid(id);

            if (_service.DeleteAction(param) == 0)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}