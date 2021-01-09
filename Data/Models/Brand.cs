using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class Brand
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
        public string DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual Account UsernameNavigation { get; set; }
    }
}
