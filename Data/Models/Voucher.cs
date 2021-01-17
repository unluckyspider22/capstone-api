﻿using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class Voucher
    {
        public Guid VoucherId { get; set; }
        public string VoucherCode { get; set; }
        public Guid? VoucherChannelId { get; set; }
        public Guid? VoucherGroupId { get; set; }
        public Guid? MembershipId { get; set; }
        public bool? IsUsed { get; set; }
        public bool? IsRedemped { get; set; }
        public DateTime? UsedDate { get; set; }
        public string IsLimited { get; set; }
        public DateTime? RedempedDate { get; set; }
        public bool? IsActive { get; set; }
        public string DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual Membership Membership { get; set; }
        public virtual VoucherChannel VoucherChannel { get; set; }
        public virtual VoucherGroup VoucherGroup { get; set; }
    }
}
