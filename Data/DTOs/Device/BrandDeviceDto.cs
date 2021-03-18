using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class BrandDeviceDto
    {
        public Guid DeviceId { get; set; }
        public Guid StoreId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public decimal? Group { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(8)]
        public string Code { get; set; }
    }
}
