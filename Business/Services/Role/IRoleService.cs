using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace ApplicationCore.Service
{
    public interface IRoleService
    {
        public List<RoleEntity> GetRoles();

        public RoleEntity GetRole(string id);
    }
}
