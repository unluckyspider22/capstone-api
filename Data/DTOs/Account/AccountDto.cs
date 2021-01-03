using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class AccountDto : BaseDto
    {
        public string Username { get; set; }
        public Guid? BrandId { get; set; }
        public int? RoleId { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
