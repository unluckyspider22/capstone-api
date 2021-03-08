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

        public async Task<List<BrandDeviceDto>> GetBrandDevice(Guid brandId)
        {
            var result = new List<BrandDeviceDto>();
            IGenericRepository<Store> storeRepo = _unitOfWork.StoreRepository;
            var stores = await storeRepo.Get(filter: o => o.BrandId.Equals(brandId) && !o.DelFlg, includeProperties: "Device");
            if (stores != null)
            {
                foreach (var store in stores)
                {
                    var devices = store.Device.ToList();
                    if (devices != null && devices.Count > 0)
                    {
                        foreach (var device in devices)
                        {
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
            return result;
        }

        public async Task<DeviceDto> Update(DeviceDto dto)
        {
            try
            {
                var entity = await _repository.GetFirst(filter: o => o.DeviceId.Equals(dto.DeviceId) && !o.DelFlg);
                if (entity != null)
                {
                    entity.UpdDate = DateTime.Now;
                    var updParam = _mapper.Map<Device>(dto);
                    if (updParam.Imei != null)
                    {
                        entity.Imei = dto.Imei;
                    }
                    if (updParam.Name != null)
                    {
                        entity.Name = dto.Name;
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
