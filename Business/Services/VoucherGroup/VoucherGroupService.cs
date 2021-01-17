
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

        public List<VoucherDto> GenerateVoucher(decimal? quantity, string charset, string prefix, string postFix, int codeLength = 10, string customCharset = "")
        {
            List<VoucherDto> result = new List<VoucherDto>();
            for (var i = 0; i < quantity; i++)
            {
                VoucherDto dto = new VoucherDto();
                string randomVoucher = RandomString(charset, customCharset,codeLength);
                dto.VoucherCode = prefix + randomVoucher + postFix;
                result.Add(dto);

            }
            return result;
        }
        private Random Random = new Random();
        public string RandomString(string charset, string customCode, int length)
        {
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
    }

}
