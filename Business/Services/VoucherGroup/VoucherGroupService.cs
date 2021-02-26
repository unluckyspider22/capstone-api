
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.Repository.Voucher;
using Infrastructure.UnitOrWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class VoucherGroupService : BaseService<VoucherGroup, VoucherGroupDto>, IVoucherGroupService
    {
        public VoucherGroupService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        protected override IGenericRepository<VoucherGroup> _repository => _unitOfWork.VoucherGroupRepository;
        protected IGenericRepository<Voucher> _repositoryVoucher => _unitOfWork.VoucherRepository;
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

        public async Task<IEnumerable<VoucherParamResponse>> GetVoucherForGame(int PageIndex = 0, int PageSize = 0, string BrandCode = null, string StoreCode = null)
        {
            try
            {
                /* var listVoucherGroup = await _repository.Get(pageIndex: PageIndex, pageSize: PageSize, includeProperties: "Voucher",
                     filter: (el =>
                      !el.DelFlg &&
                     el.VoucherType.Equals(AppConstant.ENVIRONMENT_VARIABLE.VOUCHER_TYPE.BULK_CODE)
                         && el.IsActive
                         && el.PublicDate.Value.CompareTo(DateTime.Now) <= 0
                         && el.UsedQuantity < el.Quantity
                         && el.RedempedQuantity < el.Quantity
                         //điều kiện tùy chọn để lấy voucher cho game (Promotion)
                         && !el.Promotion.PromotionType.Equals(AppConstant.ENVIRONMENT_VARIABLE.PROMOTION_TYPE.PROMOTION)
                         && el.Promotion.Status.Equals(AppConstant.ENVIRONMENT_VARIABLE.PROMOTION_STATUS.PUBLISH)
                         && el.Promotion.IsActive
                         && el.Promotion.DelFlg
                         && el.Promotion.StartDate.Value.CompareTo(DateTime.Now) <= 0
                         && el.Promotion.EndDate.Value.CompareTo(DateTime.Now) >= 0
                         //điều kiện tùy chọn để lấy voucher cho game (Brand)
                         && el.Promotion.Brand.BrandCode.Equals(BrandCode)
                         //điều kiện tùy chọn để lấy voucher cho game (Store)
                         && el.Promotion.PromotionStoreMapping.Any(x => x.Store.StoreCode.Equals(StoreCode))));*/
                var listVoucherGroup = await _repository.Get(pageIndex: PageIndex, pageSize: PageSize, includeProperties: "Voucher"
                    , filter: (el => !el.DelFlg
                    && el.VoucherType.Equals(AppConstant.EnvVar.VoucherType.BULK_CODE)
                     && el.IsActive
                     && el.PublicDate.Value.CompareTo(DateTime.Now) <= 0
                     && el.UsedQuantity < el.Quantity
                     && el.RedempedQuantity < el.Quantity
                     && !el.Promotion.PromotionType.Equals(AppConstant.EnvVar.PromotionType.PROMOTION)
                     && el.Promotion.IsActive
                     && el.Promotion.DelFlg
                         && el.Promotion.StartDate.Value.CompareTo(DateTime.Now) <= 0
                         && el.Promotion.EndDate.Value.CompareTo(DateTime.Now) >= 0
                         //điều kiện tùy chọn để lấy voucher cho game (Brand)
                         && el.Promotion.Brand.BrandCode.Equals(BrandCode)
                    //điều kiện tùy chọn để lấy voucher cho game (Store)
                    && el.Promotion.PromotionStoreMapping.Any(x => x.Store.StoreCode.Equals(StoreCode))
                    )
                                        );
                List<VoucherParamResponse> result = new List<VoucherParamResponse>();
                foreach (VoucherGroup voucherGroup in listVoucherGroup.ToList())
                {
                    int order = 0;
                    Voucher voucherInGroup = voucherGroup.Voucher.ElementAt(0);
                    //Debug.WriteLine("\n" + "voucherrrrr  " + voucherInGroup.VoucherCode.ToString());
                    // đến khi gặp được voucher thỏa điều kiện là isActive/isUsed/isRedemped/DelFlg

                    bool flag;
                    do
                    {
                        flag = false;
                        voucherInGroup = voucherGroup.Voucher.ElementAt(order);
                        if (voucherInGroup.IsActive.Equals(AppConstant.ACTIVE)
                            && voucherInGroup.IsRedemped.Equals(AppConstant.UNREDEMPED)) flag = true;
                        order++;
                    } while (flag != true);
                    //nếu lấy được voucher thì cập nhật lại IsRedemped là 1 (đã được phát đi)
                    voucherInGroup.IsRedemped = true;
                    voucherInGroup.RedempedDate = DateTime.Now;
                    voucherInGroup.UpdDate = DateTime.Now;
                    _repositoryVoucher.Update(voucherInGroup);
                    await _unitOfWork.SaveAsync();
                    //cập nhập lại số lượng đã redeemped trên Voucher Group
                    voucherGroup.UpdDate = DateTime.Now;
                    voucherGroup.RedempedQuantity = voucherGroup.RedempedQuantity + 1;
                    _repository.Update(voucherGroup);
                    await _unitOfWork.SaveAsync();
                    Debug.WriteLine("\n" + "voucherrrrr  " + voucherGroup.Voucher.Count);
                    //thêm voucher vào list result, list này để cho game
                    VoucherParamResponse uiuiu = new VoucherParamResponse(
                             voucherGroupId: voucherGroup.VoucherGroupId,
                             voucherGroupName: voucherGroup.VoucherName,
                             voucherId: voucherInGroup.VoucherId,
                             //voucherInGroup.VoucherId,
                             code: voucherInGroup.VoucherCode);
                    result.Add(uiuiu);
                    //voucherInGroup.VoucherCode));
                }
                return result;
            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at getVoucherForGame: \n" + e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
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
    }
}
