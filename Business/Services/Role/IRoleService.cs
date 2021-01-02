using Infrastructure.DTOs.Role;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace ApplicationCore.Services
{
    public interface IRoleService : IBaseService<RoleEntity, RoleDto>
    {
      
    }
}
