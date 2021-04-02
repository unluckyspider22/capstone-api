using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class GameConfigService : BaseService<GameCampaign, GameConfigDto>, IGameConfigService
    {
        public GameConfigService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<GameCampaign> _repository => _unitOfWork.GameConfigRepository;

        public async Task<bool> DeleteGameConfig(Guid id)
        {
            try
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
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.InnerException);
                throw new ErrorObj(code: 500, message: e.Message, description: "Internal Server Error");
            }


        }
        public async Task<GameConfigDto> UpdateGameConfig(GameConfigDto dto)
        {
            try
            {
                dto.UpdDate = DateTime.Now;
                var entity = _mapper.Map<GameCampaign>(dto);
                var items = entity.GameItems.ToList();
                await UpdateGameItem(items, dto.Id);
                _repository.Update(entity);
                await _unitOfWork.SaveAsync();
                return _mapper.Map<GameConfigDto>(entity);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.InnerException);
                throw new ErrorObj(code: 500, message: e.Message, description: "Internal Server Error");
            }

        }
        private async Task<bool> UpdateGameItem(List<GameItems> list, Guid gameConfigId)
        {
            IGenericRepository<GameItems> itemRepo = _unitOfWork.GameItemsRepository;
            itemRepo.Delete(id: Guid.Empty, filter: o => o.GameId.Equals(gameConfigId));
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    itemRepo.Add(item);
                }
            }
            return await _unitOfWork.SaveAsync() > 0;
        }
    }
}
