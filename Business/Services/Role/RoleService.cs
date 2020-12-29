using ApplicationCore.Service;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ApplicationCore.Service
{
    public class RoleService : IRoleService
    {
        private readonly PromotionEngineContext _context;

        public RoleService(PromotionEngineContext context)
        {
            _context = context;
        }

        public RoleEntity GetRole(string id)
        {
            throw new NotImplementedException();
        }

        public List<RoleEntity> GetRoles()
        {
            
            return _context.Role.ToList();
        }
    }
}
