
using ApplicationCore.Worker;
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
using System.Net;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class VoucherGroupService : BaseService<VoucherGroup, VoucherGroupDto>, IVoucherGroupService
    {
        private readonly IVoucherWorker _voucherWorker;
        public VoucherGroupService(IUnitOfWork unitOfWork, IMapper mapper, IVoucherWorker voucherWorker) : base(unitOfWork, mapper)
        {
            _voucherWorker = voucherWorker;
        }

        protected override IGenericRepository<VoucherGroup> _repository => _unitOfWork.VoucherGroupRepository;
        //protected IGenericRepository<Voucher> _repositoryVoucher => _unitOfWork.VoucherRepository;
        public List<VoucherDto> GenerateBulkCodeVoucher(VoucherGroupDto dto)
        {
            List<VoucherDto> result = new List<VoucherDto>();
            if (!dto.IsLimit)
            {
                for (var i = 0; i < dto.Quantity; i++)
                {
                    VoucherDto voucher = new VoucherDto();
                    string randomVoucher = RandomString(dto.Charset, dto.CustomCharset, dto.CodeLength);
                    voucher.VoucherCode = dto.Prefix + randomVoucher + dto.Postfix;
                    voucher.VoucherGroupId = dto.VoucherGroupId;
                    result.Add(voucher);
                }
            }
            else
            {
                VoucherDto voucher = new VoucherDto();
                string randomVoucher = RandomString(dto.Charset, dto.CustomCharset, dto.CodeLength);
                voucher.VoucherCode = dto.Prefix + randomVoucher + dto.Postfix;
                voucher.VoucherGroupId = dto.VoucherGroupId;
                result.Add(voucher);
            }

            return result;
        }
        private Random Random = new Random();
        public string RandomString(string charset, string customCode, int length)
        {
            if (length == 0)
            {
                length = 10;
            }
            string randomCode = "";
            string chars = "";
            switch (charset)
            {
                case AppConstant.EnvVar.CharsetType.ALPHABETIC:
                    chars = AppConstant.EnvVar.CharsetChars.ALPHABETIC;

                    randomCode = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
                    break;
                case AppConstant.EnvVar.CharsetType.ALPHANUMERIC:
                    chars = AppConstant.EnvVar.CharsetChars.ALPHANUMERIC;

                    randomCode = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
                    break;
                case AppConstant.EnvVar.CharsetType.ALPHABETIC_UPERCASE:
                    chars = AppConstant.EnvVar.CharsetChars.ALPHABETIC_UPERCASE;

                    randomCode = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
                    break;
                case AppConstant.EnvVar.CharsetType.ALPHABETIC_LOWERCASE:
                    chars = AppConstant.EnvVar.CharsetChars.ALPHABETIC_LOWERCASE;

                    randomCode = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
                    break;
                case AppConstant.EnvVar.CharsetType.NUMBERS:
                    chars = AppConstant.EnvVar.CharsetChars.NUMBERS;

                    randomCode = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
                    break;
                case AppConstant.EnvVar.CharsetType.CUSTOM:
                    chars = customCode;
                    randomCode = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
                    break;
            }
            return randomCode;
        }

        public List<VoucherDto> GenerateStandaloneVoucher(VoucherGroupDto dto)
        {
            List<VoucherDto> result = new List<VoucherDto>();
            VoucherDto voucher = new VoucherDto
            {
                VoucherCode = dto.Prefix + dto.CustomCode + dto.Postfix,
                VoucherGroupId = dto.VoucherGroupId
            };
            result.Add(voucher);
            return result;
        }

        public async Task<IEnumerable<VoucherGroup>> GetVoucherGroupForGame(int PageIndex = 0, int PageSize = 0,
            string BrandCode = null, string StoreCode = null)
        {
            try
            {
                var listVoucherGroup = await _repository.Get(pageIndex: PageIndex, pageSize: PageSize
                   , filter: (el => !el.DelFlg
                   && el.VoucherType.Equals(AppConstant.EnvVar.VoucherType.BULK_CODE)
                    && el.IsActive
                    && el.RedempedQuantity < el.Quantity
                    && !el.Promotion.PromotionType.Equals(AppConstant.EnvVar.PromotionType.AUTO_PROMOTION)
                    && el.Promotion.Status.Equals(AppConstant.EnvVar.PromotionStatus.PUBLISH)
                    && !el.Promotion.DelFlg
                        && el.Promotion.StartDate.Value.CompareTo(DateTime.Now) <= 0
                        && (el.Promotion.EndDate.Value.CompareTo(DateTime.Now) >= 0 || el.Promotion.EndDate.Value == null)
                        && el.Promotion.IsForGame == AppConstant.EnvVar.IS_FOR_GAME

                        //điều kiện tùy chọn để lấy voucher cho game (Brand)
                        && el.Promotion.Brand.BrandCode.Equals(BrandCode)
                   //điều kiện tùy chọn để lấy voucher cho game (Store)
                   && el.Promotion.PromotionStoreMapping.Any(x => x.Store.StoreCode.Equals(StoreCode))));
                if (listVoucherGroup != null && listVoucherGroup.Count() > 0)
                    return listVoucherGroup;
                else
                    throw new ErrorObj(code: 204, message: "No promotion for game exists !!!");

            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at getVoucherForGame: \n" + e.Message);
                throw new ErrorObj(code: 500, message: e.Message);
            }
        }

        public async Task<bool> DeleteVoucherGroup(Guid id)
        {
            try
            {
                //IVoucherRepository voucherRepo = new VoucherRepositoryImp();
                // Delete vouchers
                //voucherRepo.DeleteBulk(voucherGroupId: id);
                // Delete voucher group
                //_repository.Delete(id: id, filter: null);
                //return await _unitOfWork.SaveAsync() > 0;
                return true;
            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at getVoucherForGame: \n" + e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }
        }

        public async Task<bool> RejectVoucherGroup(Guid id)
        {
            try
            {
                // Reject voucher group
                var voucherGroupEntity = await _repository.GetFirst(filter: o => o.VoucherGroupId.Equals(id) && !o.DelFlg);
                if (voucherGroupEntity != null)
                {
                    voucherGroupEntity.PromotionId = null;
                    _repository.Update(voucherGroupEntity);
                }
                return await _unitOfWork.SaveAsync() > 0;
            }

            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at getVoucherForGame: \n" + e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }
        }

        public async Task<bool> AssignVoucherGroup(Guid promotionId, Guid voucherGroupId)
        {
            try
            {
                // Assign voucher group
                var voucherGroupEntity = await _repository.GetFirst(filter: o => o.VoucherGroupId.Equals(voucherGroupId) && !o.DelFlg);
                if (voucherGroupEntity != null)
                {
                    voucherGroupEntity.PromotionId = promotionId;
                    _repository.Update(voucherGroupEntity);
                }
                return await _unitOfWork.SaveAsync() > 0;
            }

            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at getVoucherForGame: \n" + e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }
        }

        public async Task<bool> HideVoucherGroup(Guid id)
        {
            try
            {
                // Hide voucher group
                var voucherGroupEntity = await _repository.GetFirst(filter: o => o.VoucherGroupId.Equals(id) && !o.DelFlg);
                if (voucherGroupEntity != null)
                {
                    voucherGroupEntity.DelFlg = true;
                    _repository.Update(voucherGroupEntity);
                }
                return await _unitOfWork.SaveAsync() > 0;
            }

            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at getVoucherForGame: \n" + e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }
        }

        public async Task<List<VoucherDto>> CreateVoucherBulk(List<VoucherDto> vouchers)
        {
            try
            {
                IVoucherRepository voucherRepository = new VoucherRepositoryImp();
                List<Voucher> listEntities = _mapper.Map<List<Voucher>>(vouchers);
                await voucherRepository.InsertBulk(listEntities);
                return vouchers;
            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at getVoucherForGame: \n" + e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }

        }

        public async Task<List<Voucher>> MapVoucher(List<VoucherDto> vouchers)
        {
            return _mapper.Map<List<Voucher>>(vouchers);
        }

        public async Task<string> GetPromotionCode(Guid promotionId)
        {
            try
            {
                IGenericRepository<Promotion> promotionRepo = _unitOfWork.PromotionRepository;
                return (await promotionRepo.GetFirst(filter: x => x.PromotionId.Equals(promotionId))).PromotionCode;
            }
            catch (Exception e)
            {
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }
        }

        public async Task UpdateRedempedQuantity(VoucherGroup voucherGroup, int RedempedQuantity)
        {
            try
            {
                if (voucherGroup != null)
                {
                    voucherGroup.RedempedQuantity += RedempedQuantity;
                    voucherGroup.UpdDate = DateTime.Now;
                    _repository.Update(voucherGroup);
                    await _unitOfWork.SaveAsync();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("aaaa " + e.Message);
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e.Message);
            }

        }

        public async Task UpdateVoucherGroupForApplied(VoucherGroup voucherGroup)
        {
            try
            {
                if (voucherGroup != null)
                {
                    _repository.Update(voucherGroup);
                    await _unitOfWork.SaveAsync();
                }
            }
            catch (Exception e)
            {

                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: e.Message);
            }

        }

        public async Task AddMoreVoucher(VoucherGroupDto dto)
        {
            var vouchers = (await _repository.GetFirst(filter: el => el.VoucherGroupId == dto.VoucherGroupId)).Voucher;
            var newVouchers = _voucherWorker.GenerateVoucher(dto);

            int quantity = vouchers.Count() + (int)dto.Quantity;
            //Gộp 2 mảng [đã có dưới DB] + [voucher code mới gen]
            var totalVouchers = newVouchers.Concat(vouchers);
            //Kiểm tra có trùng voucher code với những cái cũ hay không
            while (totalVouchers.Select(el => el.VoucherCode).Distinct().Count() < totalVouchers.Select(el => el.VoucherCode).Count())
            {
                int remainVoucher = totalVouchers.Select(el => el.VoucherCode).Count() - totalVouchers.Select(el => el.VoucherCode).Distinct().Count();
                totalVouchers = totalVouchers.Union(newVouchers);
                dto.Quantity = remainVoucher;
                //Gen lại số lượng voucher
                var remainVouchers = _voucherWorker.GenerateVoucher(dto);
                totalVouchers.ToList().AddRange(remainVouchers);
            }
            newVouchers = totalVouchers.Except(vouchers).ToList();
            _voucherWorker.InsertVouchers(dto, true, newVouchers);

            //Update lại quantity
            var voucherGroup = await _repository.GetById(dto.VoucherGroupId);
            voucherGroup.Quantity = quantity;
            _repository.Update(voucherGroup);
            await _unitOfWork.SaveAsync();

        }
    }
}
