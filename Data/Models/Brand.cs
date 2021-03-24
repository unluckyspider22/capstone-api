﻿using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Channel = new HashSet<Channel>();
            ConditionRule = new HashSet<ConditionRule>();
            Game = new HashSet<Game>();
            MemberLevel = new HashSet<MemberLevel>();
            ProductCategory = new HashSet<ProductCategory>();
            Promotion = new HashSet<Promotion>();
            Store = new HashSet<Store>();
            Transaction = new HashSet<Transaction>();
            VoucherGroup = new HashSet<VoucherGroup>();
        }

        public Guid BrandId { get; set; }
        public string Username { get; set; }
        public string BrandCode { get; set; }
        public string PhoneNumber { get; set; }
        public string ImgUrl { get; set; }
        public string BrandName { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public bool DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

        public virtual Account UsernameNavigation { get; set; }
        public virtual ICollection<Channel> Channel { get; set; }
        public virtual ICollection<ConditionRule> ConditionRule { get; set; }
        public virtual ICollection<Game> Game { get; set; }
        public virtual ICollection<MemberLevel> MemberLevel { get; set; }
        public virtual ICollection<ProductCategory> ProductCategory { get; set; }
        public virtual ICollection<Promotion> Promotion { get; set; }
        public virtual ICollection<Store> Store { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
        public virtual ICollection<VoucherGroup> VoucherGroup { get; set; }
    }
}
