using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
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
    public class DeviceService : BaseService<Device, DeviceDto>, IDeviceService
    {
        public DeviceService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Device> _repository => _unitOfWork.DeviceRepository;

        public async Task<bool> CheckExistingDevice(string imei)
        {
            var isExist = (await _repository.Get(filter: o => o.Imei.Equals(imei) && !o.DelFlg)).ToList().Count > 0;
            return isExist;
        }

        public async Task<GenericRespones<BrandDeviceDto>> GetBrandDevice(int PageSize, int PageIndex, Guid brandId)
        {
            var result = new List<BrandDeviceDto>();
            IGenericRepository<Store> storeRepo = _unitOfWork.StoreRepository;
            var stores = await storeRepo.Get(pageSize: PageSize, pageIndex: PageIndex,
                filter: o => o.BrandId.Equals(brandId) && !o.DelFlg, includeProperties: "Device");
            if (stores != null)
            {
                foreach (var store in stores)
                {
                    var devices = store.Device.ToList();
                    if (devices != null && devices.Count > 0)
                    {
                        foreach (var device in devices)
                        {
                            if (!device.DelFlg) {
                                var dto = new BrandDeviceDto()
                                {
                                    DeviceId = device.DeviceId,
                                    Imei = device.Imei,
                                    Name = device.Name,
                                    Group = store.Group,
                                    StoreCode = store.StoreCode,
                                    StoreId = store.StoreId,
                                    StoreName = store.StoreName,
                                };
                                result.Add(dto);
                            }
                        }
                    }


                }
            }


            var totalItem = await _repository.CountAsync(filter: o => o.Store.BrandId.Equals(brandId) && !o.DelFlg);
            MetaData metadata = new MetaData(pageIndex: PageIndex, pageSize: PageSize, totalItems: totalItem);

            GenericRespones<BrandDeviceDto> response = new GenericRespones<BrandDeviceDto>(data: result.ToList(), metadata: metadata);
            return response;
        }

        public async Task<DeviceDto> Update(DeviceDto dto)
        {
            try
            {
                var entity = await _repository.GetFirst(filter: o => o.DeviceId.Equals(dto.DeviceId) && !o.DelFlg);
                if (entity != null)
                {
                    entity.UpdDate = DateTime.Now;
                    //var updParam = _mapper.Map<Device>(dto);
                    if (dto.Imei != null)
                    {
                        entity.Imei = dto.Imei;
                    }
                    if (dto.Name != null)
                    {
                        entity.Name = dto.Name;
                    }
                    if (dto.DelFlg != null)
                    {
                        entity.DelFlg = dto.DelFlg;
                    }
                    if (!dto.StoreId.Equals(Guid.Empty) && !dto.StoreId.Equals(entity.StoreId))
                    {
                        IGenericRepository<Store> storeRepo = _unitOfWork.StoreRepository;
                        var oldStore = await storeRepo.GetFirst(filter: o => o.StoreId.Equals(entity.StoreId) && !o.DelFlg);
                        oldStore.Device.Remove(entity);
                        var newStore = await storeRepo.GetFirst(filter: o => o.StoreId.Equals(dto.StoreId) && !o.DelFlg);
                        entity.StoreId = dto.StoreId;
                    }
                    _repository.Update(entity);
                    await _unitOfWork.SaveAsync();
                    return _mapper.Map<DeviceDto>(entity);
                }
                else
                {
                    throw new ErrorObj(code: 500, message: "Device not found");
                }
            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at getVoucherForGame: \n" + e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }


        }
    }
}
