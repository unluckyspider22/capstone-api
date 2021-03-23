using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class DuplicateParam
    {
        public Guid BrandID { get; set; }
        public string StoreCode { get; set; }
        public string ChannelCode { get; set; }
    }
}
