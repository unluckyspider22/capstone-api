using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace ApplicationCore.Services
{
    public interface IRoleService
    {
        public List<RoleEntity> GetRoles();

        public RoleEntity GetRole(string id);
    }
}
