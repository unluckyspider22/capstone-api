using ApplicationCore.Models.PromotionTier;
using ApplicationCore.Services;
using ApplicationCore.Utils;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionTiersController : ControllerBase
    {
        private readonly IPromotionTierService _service;

        public PromotionTiersController(IPromotionTierService service)
        {
            _service = service;
        }

        // GET: api/PromotionTiers
        [HttpGet]
        public List<PromotionTier> GetPromotionTier()
        {
            var result = _service.GetPromotionTiers();
            return result;
        }

        // GET: api/PromotionTiers/5
        [HttpGet("{id}")]
        public ActionResult<PromotionTierParam> GetPromotionTier(Guid id)
        {

            PromotionTier result = _service.GetPromotionTier(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // PUT: api/PromotionTiers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public ActionResult<PromotionTier> PutPromotionTier(Guid id, PromotionTierParam PromotionTierParam)
        {
            var result = _service.PutPromotionTier(id, PromotionTierParam);
            if (result == GlobalVariables.NOT_FOUND) return NotFound();
            return Ok(PromotionTierParam);
        }
        //PATCH:  api/PromotionTiers/2?delflg=?
        [HttpPatch("{id}")]
        public ActionResult UpdateDelFlg(Guid id, string delflg)
        {
            var result = _service.UpdateDelFlag(id, delflg);
            if (result == GlobalVariables.NOT_FOUND)
                return NotFound();
            return Ok();
        }
        // POST: api/PromotionTiers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult<PromotionTier> PostPromotionTier(PromotionTier PromotionTier)
        {
            var result = _service.PostPromotionTier(PromotionTier);
            if (result == GlobalVariables.DUPLICATE) return Conflict();
            return Ok(PromotionTier);
        }

        // DELETE: api/PromotionTiers/5
        [HttpDelete("{id}")]
        public ActionResult DeletePromotionTier(Guid id)
        {
            var result = _service.DeletePromotionTier(id);
            if (result == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }
            return Ok();
        }
        // GETCOUNT: api/PromotionTiers/count
        [HttpGet]
        [Route("count")]
        public ActionResult GetPromotionTierCount()
        {
            var result = _service.CountPromotionTier();
            return Ok(result);
        }
    }
}
