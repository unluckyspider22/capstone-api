using ApplicationCore.Utils;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class GameCampaignService : BaseService<GameCampaign, GameCampaignDto>, IGameCampaignService
    {
        public GameCampaignService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
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
                    _repository.Update(config);
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

        public async Task<List<GameItemDto>> GetGameCampaignItems(Guid deviceId, string gameCode)
        {
            try
            {
                List<GameItemDto> gameItemDtos = null;

                var gameConfig = await _repository.GetFirst(filter: o =>
                    o.StoreGameCampaignMapping.Select(s => s.Store).FirstOrDefault(f => f.Device.Any(a => a.DeviceId == deviceId)) != null
                    && o.SecretCode == gameCode
                    && !o.DelFlg,
                    includeProperties: "StoreGameCampaignMapping.Store.Device,GameItems");

                /*  var gameItems = gameConfig.GameItems.Where(w => w.Promotion.Status.Equals(AppConstant.EnvVar.PromotionStatus.PUBLISH)
                  && !w.Promotion.DelFlg
                  );*/
                var gameItems = gameConfig.GameItems;
                if (gameConfig != null && gameItems.Count() > 0)
                {
                    foreach (var gameItem in gameItems)
                    {
                        var dto = _mapper.Map<GameItemDto>(gameItem);
                        dto.PromotionId = (Guid)gameConfig.PromotionId;
                        if (gameItemDtos == null)
                        {
                            gameItemDtos = new List<GameItemDto>();
                        }
                        gameItemDtos.Add(dto);
                    }
                    var totalPriority = gameConfig.GameItems.Sum(s => s.Priority);

                    //Tính tỷ lệ cho từng item
                    gameItemDtos = gameItemDtos.Select(
                        el =>
                        {
                            el.Ratio = (decimal)(el.Priority * 1.0 / totalPriority * 1.0);
                            return el;
                        }).ToList();
                }
                return gameItemDtos;
            }
            catch (Exception e)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e.Message);
            }
        }
        public async Task<GameCampaignDto> CreateGameCampaign(GameCampaignDto dto)
        {
            try
            {
                var now = Common.GetCurrentDatetime();
                dto.UpdDate = now;
                dto.InsDate = now;

                var gameCampaign = _mapper.Map<GameCampaign>(dto);
                _repository.Add(gameCampaign);
                await _unitOfWork.SaveAsync();

                var store = await GetStore(dto.StoreId);

                var mapping = new StoreGameCampaignMapping
                {
                    GameCampaign = gameCampaign,
                    Store = store
                };
                await AddMappingStoreCampaign(mapping);

                return dto;
            }
            catch (Exception e)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
        public async Task<GameCampaignDto> UpdateGameCampaign(GameCampaignDto dto)
        {
            try
            {
                dto.UpdDate = DateTime.Now;
                var entity = _mapper.Map<GameCampaign>(dto);
                var gameMaster = await GetGameMaster(dto.GameMasterId);
                if (gameMaster != null)
                {
                    entity.GameMaster = gameMaster;

                }
                var promotion = await GetPromotion(dto.PromotionId);
                if (promotion != null)
                {
                    entity.Promotion = promotion;
                }
                var items = entity.GameItems.ToList();
                await UpdateGameItem(items, dto.Id);
                _repository.Update(entity);
                await _unitOfWork.SaveAsync();
                return _mapper.Map<GameCampaignDto>(entity);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                Debug.WriteLine(e.InnerException);
                throw new ErrorObj(code: 500, message: e.Message, description: "Internal Server Error");
            }

        }
        private async Task<Store> GetStore(Guid storeId)
        {
            IGenericRepository<Store> storeRepo = _unitOfWork.StoreRepository;
            var store = await storeRepo.GetById(storeId);
            return store;
        }
        private async Task<GameMaster> GetGameMaster(Guid gameMasterId)
        {
            IGenericRepository<GameMaster> masterRepo = _unitOfWork.GameMasterRepository;
            var gameMaster = await masterRepo.GetById(gameMasterId);
            return gameMaster;
        }
        private async Task<Promotion> GetPromotion(Guid promotionId)
        {
            IGenericRepository<Promotion> promotionRepo = _unitOfWork.PromotionRepository;
            var promotion = await promotionRepo.GetById(promotionId);
            return promotion;
        }
        private async Task<bool> AddMappingStoreCampaign(StoreGameCampaignMapping mapping)
        {
            IGenericRepository<StoreGameCampaignMapping> storeGameRepo = _unitOfWork.StoreGameCampaignMappingRepository;
            storeGameRepo.Add(mapping);
            return await _unitOfWork.SaveAsync() > 0;
        }
        private async Task<bool> UpdateGameItem(List<GameItems> list, Guid gameCampaignId)
        {
            IGenericRepository<GameItems> itemRepo = _unitOfWork.GameItemsRepository;
            itemRepo.Delete(id: Guid.Empty, filter: o => o.GameId.Equals(gameCampaignId));
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
