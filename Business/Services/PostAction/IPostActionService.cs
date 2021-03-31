
using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IPostActionService : IBaseService<PostAction, PostActionDto>
    {
        public Task<PostActionDto> MyAddAction(PostActionDto dto);
        public Task<bool> Delete(Guid id);
    }
}
