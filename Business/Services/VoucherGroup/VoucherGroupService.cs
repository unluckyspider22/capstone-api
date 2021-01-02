using ApplicationCore.Models.VoucherGroup;
using ApplicationCore.Utils;
using AutoMapper;
using Infrastructure.DTOs.VoucherGroup;
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
    public class VoucherGroupService : BaseService<VoucherGroup, VoucherGroupDto>, IVoucherGroupService
    {
        public VoucherGroupService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        protected override IGenericRepository<VoucherGroup> _repository => _unitOfWork.VoucherGroupRepository;
    }
}
}
