
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;

namespace ApplicationCore.Services
{
    public class VoucherService : BaseService<Voucher, VoucherDto>, IVoucherService
    {
        public VoucherService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        protected override IGenericRepository<Voucher> _repository => _unitOfWork.VoucherRepository;
    }
}

