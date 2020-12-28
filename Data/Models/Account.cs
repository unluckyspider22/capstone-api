using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class Account
    {
        public Account()
        {
            BrandNavigation = new HashSet<Brand>();
        }

        public string Username { get; set; }
        public Guid? BrandId { get; set; }
        public int? RoleId { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual RoleEntity Role { get; set; }
        public virtual ICollection<Brand> BrandNavigation { get; set; }
    }
}
