using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IGameConfigService : IBaseService<GameConfig, GameConfigDto>
    {
        public Task<bool> DeleteGameConfig(Guid id);
    }
}
