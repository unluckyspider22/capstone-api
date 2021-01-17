
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;

namespace ApplicationCore.Services
{
    public class VoucherChannelService : BaseService<VoucherChannel, VoucherChannelDto>, IVoucherChannelService
    {
        public VoucherChannelService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        protected override IGenericRepository<VoucherChannel> _repository => _unitOfWork.VoucherChannelRepository;
    }
}

