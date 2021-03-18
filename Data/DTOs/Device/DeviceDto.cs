using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class DeviceDto : BaseDto
    {
        public Guid DeviceId { get; set; }
        public Guid StoreId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(8)]
        public string Code { get; set; }
    }
}
