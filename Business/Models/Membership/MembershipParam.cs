using System;
using System.Collections.Generic;

namespace ApplicationCore.Models
{
    public class MembershipParam
    {

        public Guid MembershipId { get; set; }
        public string MembershipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }

    }
}
