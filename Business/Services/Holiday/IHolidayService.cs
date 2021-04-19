using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IHolidayService : IBaseService<Holiday, HolidayDto>
    {
        public Task<List<Holiday>> GetHolidays();
        public Task<string> SyncHolidays();
    }
}
