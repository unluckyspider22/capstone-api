using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;
using ApplicationCore.Services;

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
        public PromotionStoreMapping GetPromotionStoreMapping(Guid id)
        {
            return _service.GetPromotionStoreMapping(id);
        }

        // PUT: api/PromotionStoreMappings/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        public ActionResult PutPromotionStoreMapping( PromotionStoreMapping promotionStoreMapping)
        {
            if (_service.PutPromotionStoreMapping(promotionStoreMapping) == 0) return Conflict();
            else return Ok();
        }

        // POST: api/PromotionStoreMappings
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult PostPromotionStoreMapping(PromotionStoreMapping promotionStoreMapping)
        {
            if (_service.PostPromotionStoreMapping(promotionStoreMapping) == 0) return Conflict();
            else return Ok();
        }

        // DELETE: api/PromotionStoreMappings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PromotionStoreMapping>> DeletePromotionStoreMapping(Guid id)
        {
            if (_service.DeletePromotionStoreMapping(id) == 0) return NotFound();
            else return Ok();
        }

        
    }
}
