﻿using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class DeviceService : BaseService<Device, DeviceDto>, IDeviceService
    {
        private IConfiguration _config;

        public DeviceService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : base(unitOfWork, mapper)
        {
            _config = configuration;
        }

        protected override IGenericRepository<Device> _repository => _unitOfWork.DeviceRepository;

        public async Task<bool> CheckExistingDevice(string code)
        {
            var isExist = (await _repository.Get(filter: o => o.Code.Equals(code) && !o.DelFlg)).ToList().Count > 0;
            return isExist;
        }

        public async Task<GenericRespones<BrandDeviceDto>> GetBrandDevice(int PageSize, int PageIndex, Guid brandId)
        {
            try
            {
                var result = new List<BrandDeviceDto>();

                var devices = (await _repository.Get(pageIndex: PageIndex, pageSize: PageSize,
                    filter: o => o.Store.BrandId.Equals(brandId)
                            && !o.DelFlg,
                    includeProperties: "Store"
                )).ToList();
                if (devices.Count > 0)
                {
                    foreach (var device in devices)
                    {
                        var dto = new BrandDeviceDto()
                        {
                            DeviceId = device.DeviceId,
                            Code = device.Code,
                            Name = device.Name,
                            Group = device.Store.Group,
                            StoreCode = device.Store.StoreCode,
                            StoreId = device.Store.StoreId,
                            StoreName = device.Store.StoreName,
                            //GameConfigId = device.GameConfigId,
                        };
                        //dto.GameConfigName = await GetConfigName(device.GameConfigId);
                        result.Add(dto);
                    }
                }


                var totalItem = await _repository.CountAsync(filter: o => o.Store.BrandId.Equals(brandId) && !o.DelFlg);
                MetaData metadata = new MetaData(pageIndex: PageIndex, pageSize: PageSize, totalItems: totalItem);

                GenericRespones<BrandDeviceDto> response = new GenericRespones<BrandDeviceDto>(data: result.ToList(), metadata: metadata);
                return response;
            }
            catch (Exception e)
            {
                throw new ErrorObj(code: 500, message: e.Message);
            }

        }

        private async Task<string> GetConfigName(Guid? gameConfigId)
        {
            try
            {
                IGenericRepository<GameCampaign> configRepo = _unitOfWork.GameConfigRepository;
                var config = await configRepo.GetFirst(filter: o => o.Id.Equals(gameConfigId));
                if (config != null)
                {
                    return config.Name;
                }
                return "";
            }
            catch (Exception e)
            {
                throw new ErrorObj(code: 500, message: e.Message);
            }

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
                    if (dto.Name != null)
                    {
                        entity.Name = dto.Name;
                    }
                    entity.DelFlg = dto.DelFlg;
                    //entity.GameConfigId = dto.GameConfigId;
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
                throw new ErrorObj(code: 500, message: e.Message);
            }


        }

        public async Task<PairResponseDto> GetTokenDevice(string deviceCode)
        {

            try
            {
                var isExist = await _repository.GetFirst(
                    filter: o => o.Code.Equals(deviceCode) && !o.DelFlg) != null;
                if (!isExist)
                {
                    throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Device_Access_Fail);
                }
                else
                {
                    var result = new PairResponseDto();
                    var device = await _repository.GetFirst(
                        filter: o => o.Code.Equals(deviceCode)
                                && !o.DelFlg
                                && !o.Store.DelFlg
                                && !o.Store.Brand.DelFlg,
                        includeProperties: "Store.Brand");
                    if (device != null)
                    {
                        var token = GenerateTokenDevice(device.Name);
                        result.BrandCode = device.Store.Brand.BrandCode;
                        result.DeviceCode = device.Code;
                        result.StoreCode = device.Store.StoreCode;
                        result.Token = token;
                        return result;
                    }
                    else
                    {
                        throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Device_Access_Fail);
                    }
                }
            }
            catch (Exception e)
            {
                if (e.GetType() != typeof(ErrorObj))
                {
                    throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: AppConstant.ErrMessage.Device_Access_Server_Fail);
                }
                else
                {
                    throw e;
                }

            }
        }

        private string GenerateTokenDevice(string name)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["AppSettings:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var claims = new[]
            {
                    new Claim(ClaimTypes.Name, name),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = credentials
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }
        public string GenerateCode(Guid deviceId)
        {
            try
            {
                MD5 md5Hasher = MD5.Create();
                Random rd = new Random();
                var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(deviceId.ToString()));
                var ivalue = BitConverter.ToInt16(hashed, 0);
                if (ivalue < 0)
                {
                    ivalue *= -1;
                }

                var result = ivalue.ToString();
                while (result.Length < 6)
                {
                    result += (rd.Next(0, 9).ToString());
                }
                return result;
            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at getVoucherForGame: \n" + e.InnerException);
                throw new ErrorObj(code: 500, message: e.Message);
            }

        }
    }
}
