using ApplicationCore.Models;
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
    public class MembershipActionService : BaseService<MembershipAction, MembershipActionDto>, IMembershipActionService
    {
        public MembershipActionService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<MembershipAction> _repository => _unitOfWork.MembershipActionRepository;
    }
}
