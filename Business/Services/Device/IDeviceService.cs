using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IDeviceService : IBaseService<Device, DeviceDto>
    {
        public Task<bool> CheckExistingDevice(string imei);
        public Task<DeviceDto> Update(DeviceDto dto);
        public Task<List<BrandDeviceDto>> GetBrandDevice(Guid brandId);
    }
}
