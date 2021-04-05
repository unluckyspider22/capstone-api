using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IGameCampaignService : IBaseService<GameCampaign, GameConfigDto>
    {
        public Task<bool> DeleteGameConfig(Guid id);
        public Task<GameConfigDto> UpdateGameConfig(GameConfigDto dto);
        Task<List<GameItemDto>> GetGameCampaignItems(Guid deviceId);
    }
}
