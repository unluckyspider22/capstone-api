﻿using Infrastructure.DTOs;
using System;
using System.Threading.Tasks;


namespace ApplicationCore.Services
{


    public interface IActionService : IBaseService<Infrastructure.Models.Action, ActionDto>
    {
        public Task<ActionDto> MyAddAction(ActionDto dto);
        public Task<ActionDto> GetActionDetail(Guid id);
        public Task<bool> Delete(Guid id);

        //public Task<ActionDto> UpdateAction(ActionDto dto);
    }
}
