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
    public class MemberLevelService : BaseService<MemberLevel, MemberLevelDto>, IMemberLevelService
    {
        public MemberLevelService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<MemberLevel> _repository => _unitOfWork.MemberLevelRepository;

        public async Task<bool> CheckExistingLevel(string name, Guid brandId)
        {
            var isExist = (await _repository.Get(filter: o => o.Name.ToLower().Equals(name)
            && !o.DelFlg
            && o.BrandId.Equals(brandId))).ToList().Count > 0;
            return isExist;
        }

        public async Task<MemberLevelDto> Update(MemberLevelDto dto)
        {
            try
            {
                var entity = await _repository.GetFirst(filter: o => o.MemberLevelId.Equals(dto.MemberLevelId)
                && !o.DelFlg);
                if (entity != null)
                {
                    entity.UpdDate = DateTime.Now;
                    var updParam = _mapper.Map<MemberLevel>(dto);
                    if (updParam.Name != null)
                    {
                        entity.Name = dto.Name;
                    }
                    if (updParam.DelFlg != null)
                    {
                        entity.DelFlg = dto.DelFlg;
                    }
                    _repository.Update(entity);
                    await _unitOfWork.SaveAsync();
                    return _mapper.Map<MemberLevelDto>(entity);
                }
                else
                {
                    throw new ErrorObj(code: 500, message: "Level not found");
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
