﻿using System;
using System.Collections.Generic;

namespace Infrastructure.DTOs
{
    public class VoucherGroupDto : BaseDto
    {
        public Guid VoucherGroupId { get; set; }
        public Guid? PromotionId { get; set; }
        public Guid? BrandId { get; set; }
        public string VoucherName { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UsedQuantity { get; set; }
        public decimal? RedempedQuantity { get; set; }
        public string Charset { get; set; }
        public string Postfix { get; set; }
        public string Prefix { get; set; }
        public string CustomCharset { get; set; }
        public Guid? ConditionRuleId { get; set; }
        public Guid? ActionId { get; set; }
        public Guid? PostActionId { get; set; }
        public int CodeLength { get; set; }
        public virtual ICollection<VoucherDto> Voucher { get; set; }
        public virtual ICollection<VoucherChannelDto> VoucherChannel { get; set; }

    }
    public class VoucherGroupForPromo
    {
        public Guid VoucherGroupId { get; set; }
        public decimal? Quantity { get; set; }
        public string VoucherName { get; set; }
        public Guid? ActionId { get; set; }
        public Guid? PostActionId { get; set; }
        public int ActionType { get; set; }
        public int PostActionType { get; set; }
    }

    public class VoucherIndexInfo
    {
        public int FromIndex { get; set; }
        public int MaxIndex { get; set; }
        public bool Available { get; set; }
    }

    public class VoucherGroupDetailDto
    {
        public Guid VoucherGroupId { get; set; }
        public Guid BrandId { get; set; }
        public string VoucherName { get; set; }
        public int Total { get; set; }
        public int UsedQuantity { get; set; }
        public int RedempedQuantity { get; set; }
        public int Remain { get; set; }
        public Guid? ConditionRuleId { get; set; }
        public Guid? ActionId { get; set; }
        public int ActionType { get; set; } = 0;
        public Guid? PostActionId { get; set; }
        public int PostActionType { get; set; } = 0;
        public List<PromoOfVoucher> PromoList { get; set; } = new List<PromoOfVoucher>();
    }

    public class PromoOfVoucher
    {
        public Guid PromotionId { get; set; }
        public string PromoName { get; set; }
        public string PromoCode { get; set; }
    }
}
