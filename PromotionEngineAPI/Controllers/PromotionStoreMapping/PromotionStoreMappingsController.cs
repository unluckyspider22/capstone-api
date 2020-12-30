using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;
using ApplicationCore.Services;
using ApplicationCore.Utils;
using ApplicationCore.Models.PromotionStoreMapping;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionStoreMappingsController : ControllerBase
    {
        private readonly IPromotionStoreMappingService _service;

        public PromotionStoreMappingsController(IPromotionStoreMappingService service)
        {
            _service = service;
        }

        // GET: api/PromotionStoreMappings
        [HttpGet]
        public  List<PromotionStoreMapping> GetPromotionStoreMapping()
        {
            return  _service.GetPromotionStoreMappings();
        }

        // GET: api/PromotionStoreMappings/5
        [HttpGet("{id}")]
        public ActionResult<PromotionStoreMappingParam>  GetPromotionStoreMapping(Guid id)
        {
            PromotionStoreMapping result = _service.GetPromotionStoreMapping(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // PUT: api/PromotionStoreMappings/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public ActionResult PutPromotionStoreMapping(Guid id, PromotionStoreMapping promotionStoreMapping)
        {
            var result = _service.PutPromotionStoreMapping(id, promotionStoreMapping);
            if (result == GlobalVariables.NOT_FOUND) return NotFound();
            return Ok(promotionStoreMapping);
        }
        //PATCH:  api/PromotionStoreMapping/2?delflg=?
        [HttpPatch("{id}")]
        public ActionResult UpdateDelFlg(Guid id, string delflg)
        {
            var result = _service.UpdateDelFlag(id, delflg);
            if (result == GlobalVariables.NOT_FOUND)
                return NotFound();
            return Ok();
        }

        // POST: api/PromotionStoreMappings
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult PostPromotionStoreMapping(PromotionStoreMapping promotionStoreMapping)
        {
            var result = _service.PostPromotionStoreMapping(promotionStoreMapping);
            if (result == GlobalVariables.DUPLICATE) return Conflict();
            return Ok(promotionStoreMapping);
        }

        // DELETE: api/PromotionStoreMappings/5
        [HttpDelete("{id}")]
        public ActionResult DeletePromotionStoreMapping(Guid id)
        {
            var result = _service.DeletePromotionStoreMapping(id);
            if (result == GlobalVariables.NOT_FOUND)
            {
                return NotFound();
            }
            return Ok();
        }

        
    }
}
