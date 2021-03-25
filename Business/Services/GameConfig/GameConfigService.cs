using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;

namespace ApplicationCore.Services
{
    public class GameConfigService : BaseService<GameConfig, GameConfigDto>, IGameConfigService
    {
        public GameConfigService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<GameConfig> _repository => _unitOfWork.GameConfigRepository;
    }
}
