﻿using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class Channel
    {
        public Channel()
        {
            VoucherChannel = new HashSet<VoucherChannel>();
        }

        public Guid ChannelId { get; set; }
        public Guid? BrandId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelCode { get; set; }
        public string DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual ICollection<VoucherChannel> VoucherChannel { get; set; }
    }
}
