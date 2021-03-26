using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class GameConfigService : BaseService<GameConfig, GameConfigDto>, IGameConfigService
    {
        public GameConfigService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<GameConfig> _repository => _unitOfWork.GameConfigRepository;

        public async Task<bool> DeleteGameConfig(Guid id)
        {
            IGenericRepository<GameItems> itemRepo = _unitOfWork.GameItemsRepository;
            var items = (await itemRepo.Get(filter: o => o.GameId.Equals(id))).ToList();
            if (items.Count > 0)
            {
                foreach (var item in items)
                {
                    item.DelFlg = true;
                    item.UpdDate = DateTime.Now;
                    itemRepo.Update(item);
                }
            }
            var config = await _repository.GetFirst(filter: o => o.Id.Equals(id));
            if (config != null)
            {
                config.DelFlg = true;
                config.UpdDate = DateTime.Now;
            }
            return await _unitOfWork.SaveAsync() > 0;

        }
    }
}
