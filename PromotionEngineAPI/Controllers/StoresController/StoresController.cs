using ApplicationCore.Services;
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
        public class StoresController : ControllerBase
        {
            private readonly IStoreService _service;

            public StoresController(IStoreService service)
            {
                _service = service;
            }

            // GET: api/Stores
            [HttpGet]
            public List<Store> GetStore()
            {
                return _service.GetStores();
            }

            // GET: api/Stores/5
            [HttpGet("{id}")]
            public Store GetStore(Guid id)
            {
                return _service.GetStore(id);
            }

            // PUT: api/Stores/5
            // To protect from overposting attacks, please enable the specific properties you want to bind to, for
            // more details see https://aka.ms/RazorPagesCRUD.
            [HttpPut]
            public ActionResult PutStore(Store Store)
            {
                if (_service.PutStore(Store) == 0) return Conflict();
                else return Ok();
            }

            // POST: api/Stores
            // To protect from overposting attacks, please enable the specific properties you want to bind to, for
            // more details see https://aka.ms/RazorPagesCRUD.
            [HttpPost]
            public ActionResult PostStore(Store Store)
            {
                if (_service.PostStore(Store) == 0) return Conflict();
                else return Ok();
            }

            // DELETE: api/Stores/5
            [HttpDelete("{id}")]
            public async Task<ActionResult<Store>> DeleteStore(Guid id)
            {
                if (_service.DeleteStore(id) == 0) return NotFound();
                else return Ok();
            }
        }
    }

