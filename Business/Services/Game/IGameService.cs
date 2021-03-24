using Infrastructure.DTOs;
using Infrastructure.Models;

namespace ApplicationCore.Services
{
    public interface IGameService : IBaseService<Game, GameDto>
    {
    }
}
