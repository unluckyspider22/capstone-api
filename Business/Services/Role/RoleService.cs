using ApplicationCore.Service;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ApplicationCore.Repository.Role
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
