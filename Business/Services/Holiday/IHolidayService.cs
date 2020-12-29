using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IHolidayService
    {
        public List<Holiday> GetHoliday();
        public Holiday GetHolidays(Guid id);
        public int CreateHoliday(Holiday holiday);
        public int UpdateHoliday(Guid id, Holiday holiday);
        public int DeleteHoliday(Guid id);
        public int CountHoliday();
    }
}
