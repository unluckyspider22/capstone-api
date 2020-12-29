using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Services.Brands;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _service;

        public BrandsController(IBrandService service)
        {
            _service = service;
        }

        // GET: api/brands
        [HttpGet]
        public List<Brand> GetBrand()
        {
            return _service.GetBrands();
        }

        // GET: api/brands/count
        [HttpGet]
        [Route("count")]
        public int CountBrand()
        {
            return _service.CountBrand();
        }

        // GET: api/brands/5
        [HttpGet("{id}")]
        public Brand GetBrand(Guid id)
        {
            return _service.GetBrands(id);
        }

        // PUT: api/brands/5
        [HttpPut("{id}")]
        public ActionResult<Brand> PutBrand(Guid id, BrandParam brandParam)
        {

            if (!id.Equals(brandParam.BrandId))
            {
                return BadRequest();
            }

            if (_service.UpdateBrand(id, brandParam) == 0)
            {
                return NotFound();
            }

            return Ok(brandParam);
        }

        // POST: api/Brands
        [HttpPost]
        public ActionResult<Brand> PostBrand(BrandParam brandParam)
        {
            brandParam.BrandId = Guid.NewGuid();

            if (_service.CreateBrand(brandParam) == 0)
            {
                return Conflict();
            }
            return Ok(brandParam);
        }

        // DELETE: api/Brands/5
        [HttpDelete("{id}")]
        public ActionResult<Brand> DeleteBrand(Guid id)
        {

            if (_service.DeleteBrand(id) == 0)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}