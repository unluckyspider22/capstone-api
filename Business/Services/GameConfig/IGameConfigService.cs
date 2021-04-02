using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IGameConfigService : IBaseService<GameCampaign, GameConfigDto>
    {
        public Task<bool> DeleteGameConfig(Guid id);
        public Task<GameConfigDto> UpdateGameConfig(GameConfigDto dto);
    }
}
