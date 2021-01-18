
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Services
{
    public class VoucherGroupService : BaseService<VoucherGroup, VoucherGroupDto>, IVoucherGroupService
    {
        public VoucherGroupService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        protected override IGenericRepository<VoucherGroup> _repository => _unitOfWork.VoucherGroupRepository;

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
    }

}
