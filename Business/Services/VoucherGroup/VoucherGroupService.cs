﻿
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
            if (dto.IsLimit.Equals(AppConstant.ENVIRONMENT_VARIABLE.ISLIMIT))
            {
                for (var i = 0; i < dto.Quantity; i++)
                {
                    VoucherDto voucher = new VoucherDto();
                    string randomVoucher = RandomString(dto.Charset, dto.CustomCharset, dto.CodeLength);
                    voucher.VoucherCode = dto.Prefix + randomVoucher + dto.Postfix;
                    result.Add(voucher);
                }
            }
            else
            {
                VoucherDto voucher = new VoucherDto();
                string randomVoucher = RandomString(dto.Charset, dto.CustomCharset, dto.CodeLength);
                voucher.VoucherCode = dto.Prefix + randomVoucher + dto.Postfix;
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
                case AppConstant.ENVIRONMENT_VARIABLE.CHARSET_TYPE.ALPHABETIC:
                    chars = AppConstant.ENVIRONMENT_VARIABLE.CHARSET_CHARS.ALPHABETIC;

                    randomCode = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
                    break;
                case AppConstant.ENVIRONMENT_VARIABLE.CHARSET_TYPE.ALPHANUMERIC:
                    chars = AppConstant.ENVIRONMENT_VARIABLE.CHARSET_CHARS.ALPHANUMERIC;

                    randomCode = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
                    break;
                case AppConstant.ENVIRONMENT_VARIABLE.CHARSET_TYPE.ALPHABETIC_UPERCASE:
                    chars = AppConstant.ENVIRONMENT_VARIABLE.CHARSET_CHARS.ALPHABETIC_UPERCASE;

                    randomCode = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
                    break;
                case AppConstant.ENVIRONMENT_VARIABLE.CHARSET_TYPE.ALPHABETIC_LOWERCASE:
                    chars = AppConstant.ENVIRONMENT_VARIABLE.CHARSET_CHARS.ALPHABETIC_LOWERCASE;

                    randomCode = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
                    break;
                case AppConstant.ENVIRONMENT_VARIABLE.CHARSET_TYPE.NUMBERS:
                    chars = AppConstant.ENVIRONMENT_VARIABLE.CHARSET_CHARS.NUMBERS;

                    randomCode = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
                    break;
                case AppConstant.ENVIRONMENT_VARIABLE.CHARSET_TYPE.CUSTOM:
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
                VoucherCode = dto.Prefix + dto.CustomCode + dto.Postfix
            };
            result.Add(voucher);
            return result;
        }

        public async Task<IEnumerable<VoucherParamResponse>> GetVoucherForGame(int PageIndex = 0, int PageSize = 0, string BrandCode = null, string StoreCode = null)
        {
            try
            {
                var listVoucherGroup = await _repository.Get(pageIndex:PageIndex, pageSize:PageSize,includeProperties: "Voucher", filter: el =>
                 //điều kiện tiên quyết để lấy voucher cho game (VoucherGroup)
                 el.DelFlg.Equals("0")
                 && el.VoucherType.Equals("1")
                 && el.IsActive.Equals("1")
                 && el.PublicDate.Value.CompareTo(DateTime.Now) <= 0
                 && el.UsedQuantity < el.Quantity
                 && el.RedempedQuantity < el.Quantity
                 //điều kiện tùy chọn để lấy voucher cho game (Promotion)
                 && !el.Promotion.PromotionType.Equals("2")
                 && el.Promotion.Status.Equals("2")
                 && el.Promotion.IsActive.Equals("1")
                 && el.Promotion.DelFlg.Equals("0")
                 && el.Promotion.StartDate.Value.CompareTo(DateTime.Now) <= 0
                 && el.Promotion.EndDate.Value.CompareTo(DateTime.Now) >= 0
                 //điều kiện tùy chọn để lấy voucher cho game (Brand)
                 && el.Promotion.Brand.BrandCode.Equals(BrandCode)
                 //điều kiện tùy chọn để lấy voucher cho game (Store)
                 && el.Promotion.PromotionStoreMapping.Any(x => x.Store.StoreCode.Equals(StoreCode)));
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
                            && voucherInGroup.IsRedemped.Equals(AppConstant.UNREDEMPED)
                                && voucherInGroup.DelFlg.Equals(AppConstant.DelFlg.UNHIDE)) flag = true;
                        order++;
                    } while (flag != true);
                    //nếu lấy được voucher thì cập nhật lại IsRedemped là 1 (đã được phát đi)
                    voucherInGroup.IsRedemped = "1";
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
    }
}
