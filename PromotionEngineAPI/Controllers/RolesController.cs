using ApplicationCore.Service;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PromotionEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : Controller
    {
        private readonly IRoleService _service;

        public RolesController(IRoleService service)
        {
            _service = service;
        }

        // GET: api/Roles
        [HttpGet]
        public List<RoleEntity> GetRole()
        {
            return _service.GetRoles();
            
        }

      
    }
}
