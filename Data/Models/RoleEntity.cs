using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public partial class RoleEntity
    {
        public RoleEntity()
        {
            Account = new HashSet<Account>();
        }
        [Key]
        public int RoleId { get; set; }
        public string Name { get; set; }
        public bool DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual ICollection<Account> Account { get; set; }
    }
}
