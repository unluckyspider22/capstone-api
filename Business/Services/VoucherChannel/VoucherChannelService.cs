using ApplicationCore.Models.VoucherChannel;
using ApplicationCore.Utils;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

