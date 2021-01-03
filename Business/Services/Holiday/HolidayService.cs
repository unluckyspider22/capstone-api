using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Utils;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services
{
    public class HolidayService : BaseService<Holiday, HolidayDto>, IHolidayService
    {
        public HolidayService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Holiday> _repository => _unitOfWork.HolidayRepository;
    }
}
