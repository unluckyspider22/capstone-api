using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class HolidayDto : BaseDto
    {
        public Guid HolidayId { get; set; }
        public string HolidayName { get; set; }
        public DateTime? Date { get; set; }
        public string Rank { get; set; }
    }
}
