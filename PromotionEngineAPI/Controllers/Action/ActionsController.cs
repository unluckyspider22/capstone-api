using System.Collections.Generic;
using ApplicationCore.Models;
using ApplicationCore.Services;
using ApplicationCore.Utils;
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
        public Action GetAction(System.Guid id)
        {
            return _service.GetActions(id);
        }

        // PUT: api/Actions/5
        [HttpPut("{id}")]
        public ActionResult<Action> PutAction(System.Guid id, ActionParam actionParam)
        {

            if (!id.Equals(actionParam.ActionId))
            {
                return BadRequest();
            }

            if (_service.UpdateAction(id, actionParam) == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }

            return Ok(actionParam);
        }

        // POST: api/Actions
        [HttpPost]
        public ActionResult<Action> PostAction(ActionParam actionParam)
        {
            _service.CreateAction(actionParam);

            return Ok(actionParam);
        }

        // DELETE: api/Actions/5
        [HttpDelete("{id}")]
        public ActionResult<Action> DeleteAction(System.Guid id)
        {
            if (_service.DeleteAction(id) == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }
            return Ok();
        }

        // Patch: api/Actions/5
        [HttpPatch("{id}")]
        public ActionResult<Action> HideAction(System.Guid id)
        {
            if (_service.HideAction(id) == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}