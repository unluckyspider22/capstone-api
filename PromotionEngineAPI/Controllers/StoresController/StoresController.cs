using ApplicationCore.Models;
using ApplicationCore.Models.Store;
using ApplicationCore.Services;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Utils;

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
            var result = _service.GetStores();
            return result;
        }

        // GET: api/Stores/5
        [HttpGet("{id}")]
        public ActionResult<StoreParam> GetStore(Guid id)
        {

            StoreParam result = _service.GetStore(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // PUT: api/Stores/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        public ActionResult<Store> PutStore(Store store)
        {
            var result = _service.PutStore(store);
            if (result == GlobalVariables.NOT_FOUND) return NotFound();
            return Ok(store);
        }
        //PATCH:  api/Stores/2?delflg=?
        [HttpPatch("{id}")]
        public ActionResult UpdateDelFlg(Guid id, string delflg)
        {
            var result = _service.UpdateDelFlag(id, delflg);
            if (result == GlobalVariables.NOT_FOUND)
                return NotFound();
            return Ok();
        }
        // POST: api/Stores
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult<Store> PostStore(Store store)
        {
            var result = _service.PostStore(store);
            if (result == GlobalVariables.DUPLICATE) return Conflict();
            return Ok(store);
        }

        // DELETE: api/Stores/5
        [HttpDelete("{id}")]
        public ActionResult DeleteStore(Guid id)
        {       
            var result = _service.DeleteStore(id);
            if (result == GlobalVariables.NOT_FOUND) {
                return NotFound();
            }
            return Ok();
        }
        // GETCOUNT: api/Stores/count
        [HttpGet]
        [Route("count")]
        public ActionResult GetStoreCount()
        {
            var result = _service.CountStore();
            return Ok(result);
        }
    }
}

