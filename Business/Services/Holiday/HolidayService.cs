
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class HolidayService : BaseService<Holiday, HolidayDto>, IHolidayService
    {
        public HolidayService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<Holiday> _repository => _unitOfWork.HolidayRepository;

        public async Task<List<Holiday>> GetHolidays()
        {
            return (await _repository.Get()).ToList();
        }
    }
}
