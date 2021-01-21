
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class VoucherService : BaseService<Voucher, VoucherDto>, IVoucherService
    {
        public VoucherService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        protected override IGenericRepository<Voucher> _repository => _unitOfWork.VoucherRepository;

        public async Task<int> activeAllVoucherInGroup(VoucherGroupDto Dto)
        {
            try
            {
                int result = 0;
                var listVoucher = await _repository.Get(filter: el => el.IsActive.Equals("0") || el.IsActive == null
                && el.VoucherGroupId.Equals(Dto.VoucherGroupId));
                foreach (Voucher voucher in listVoucher.ToList())
                {
                    voucher.UpdDate = DateTime.Now;
                    voucher.IsActive = "1";
                    _repository.Update(voucher);
                    await _unitOfWork.SaveAsync();
                    result++;
                }
                return result;
            }
            catch (Exception e)
            {

                Debug.WriteLine("\n\nError at activeAllVoucherInGroup: \n" + e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.", description: "Internal Server Error");
            }
            
        }
    }
}

