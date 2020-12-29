using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class BrandParam
    {
        public Guid BrandId { get; set; }
        public string Username { get; set; }
        public string BrandCode { get; set; }
        public string PhoneNumber { get; set; }
        public string ImgUrl { get; set; }
        public string BrandName { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
    }
}
