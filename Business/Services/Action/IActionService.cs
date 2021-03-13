using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.DTOs;
using Infrastructure.Models;


namespace ApplicationCore.Services
{


    public interface IActionService : IBaseService<Infrastructure.Models.Action, ActionDto>
    {
        public Task<bool> Delete(Guid id);
    }
}
