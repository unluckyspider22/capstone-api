using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs.Account
{
    public class LoginResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public UserInfo Data { get; set; }

    }
    public class UserInfo
    {
        public string Token { get; set; }
        public string BrandCode { get; set; }
        public Guid BrandId { get; set; }
    }
}
