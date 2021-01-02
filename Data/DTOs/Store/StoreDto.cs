using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs.Store
{
    public class StoreDto : BaseDto
    {
        public Guid StoreId { get; set; }
        public Guid? BrandId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
    }
}
