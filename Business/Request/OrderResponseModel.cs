﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Request
{
    public class OrderResponseModel
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public Order Order { get; set; }

    }
    public class Order
    {
        public Order()
        {
            Gift = new List<Object>();
        }
        public List<Effect> Effects { get; set; }
        public CustomerOrderInfo CustomerOrderInfo { get; set; }

        public List<object> Gift { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountOrderDetail { get; set; }
        public decimal? FinalAmount { get; set; }
        public decimal? BonusPoint { get; set; }

    }
    public class Effect
    {
        public Guid PromotionId { get; set; }
        public Guid? PromotionTierId { get; set; }
        public int TierIndex { get; set; }
        public string PromotionName { get; set; }
        public string ConditionRuleName { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }
        public string EffectType { get; set; }
        public Object Prop { get; set; }
    }

    public class CustomerOrderInfo
    {
        public CustomerOrderInfo()
        {
            CartItems = new List<Item>();
            Customer = new Customer();
        }
        public string Id { get; set; }
        public DateTime BookingDate { get; set; }

        public OrderAttribute Attributes { get; set; }
        public List<Item> CartItems { get; set; }

        public List<CouponCode> Vouchers { get; set; }
        public decimal Amount { get; set; }
        public decimal ShippingFee { get; set; }
        public Customer Customer { get; set; }

    }
    public class OrderAttribute
    {
        public int SalesMode { get; set; }
        [StringLength(1000)]
        public string Note { get; set; }
        public int PaymentMethod { get; set; }
        public StoreInfo StoreInfo { get; set; }
    }
    public class OrderGift
    {
        [StringLength(20)]

        public string ProductCode { get; set; }
        [StringLength(20)]

        public string VoucherCode { get; set; }
        [StringLength(20)]

        public string GameCode { get; set; }
        public int Quantity { get; set; }
        [StringLength(100)]

        public string ProductName { get; set; }
    }
    public class CouponCode
    {
        [StringLength(6)]
        public string PromotionCode { get; set; }
        [StringLength(20)]
        public string VoucherCode { get; set; }
    }
    public class StoreInfo
    {
        public string StoreId { get; set; }
        [StringLength(100)]
        public string StoreName { get; set; }
        public string BrandCode { get; set; }
        public string Applier { get; set; }
        public string IpAddress { get; set; }
    }
    public class Item
    {
        [StringLength(20)]
        public string ProductCode { get; set; }
        [StringLength(20)]
        public string CategoryCode { get; set; }
        [StringLength(100)]
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountFromOrder { get; set; }
        public decimal Total { get; set; }
        [StringLength(1000)]
        public string UrlImg { get; set; }
    }
    public class Customer
    {
        [StringLength(100)]
        public string CustomerName { get; set; }
        [StringLength(100)]
        public string CustomerEmail { get; set; }
        [StringLength(11)]
        public string CustomerPhoneNo { get; set; }
        public int CustomerGender { get; set; } = 3;
        [StringLength(100)]
        public string CustomerLevel { get; set; }
    }
}
