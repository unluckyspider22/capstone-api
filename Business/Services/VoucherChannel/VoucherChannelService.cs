
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.DTOs.VoucherChannel;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

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

