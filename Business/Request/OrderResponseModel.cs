using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Request
{
    public class OrderResponseModel
    {
        public OrderResponseModel()
        {
            Gift = new List<Gift>();
        }
        public List<Effect> Effects { get; set; }
        public CustomerOrderInfo CustomerOrderInfo { get; set; }

        public List<Gift> Gift { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountOrderDetail { get; set; }
        public decimal? FinalAmount { get; set; }
        public decimal? BonusPoint { get; set; }

    }
    public class Effect
    {
        public Guid PromotionId { get; set; }
        public Guid PromotionTierId { get; set; }
        public int TierIndex { get; set; }
        public string ConditionRuleName { get; set; }
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
        public int? Id { get; set; }
        public DateTime BookingDate { get; set; }

        public Attribute Attributes { get; set; }
        public List<Item> CartItems { get; set; }

        public List<CouponCode> Vouchers { get; set; }
        public decimal Amount { get; set; }
        public decimal ShippingFee { get; set; }
        public Customer Customer { get; set; }

    }
    public class Attribute
    {
        public int SalesMode { get; set; }
        public string Note { get; set; }
        public int PaymentMethod { get; set; }
        public StoreInfo StoreInfo { get; set; }
    }
    public class Gift
    {
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
    }
    public class CouponCode
    {
        public string PromotionCode { get; set; }
        public string VoucherCode { get; set; }
    }
    public class StoreInfo
    {
        public string StoreId { get; set; }
        public string StoreName { get; set; }
        public string BrandCode { get; set; }
        public string Applier { get; set; }
        public string IpAddress { get; set; }
    }
    public class Item
    {
        public string ProductCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountFromOrder { get; set; }
        public decimal FinalAmount { get; set; }

        public string UrlImg { get; set; }
    }
    public class Customer
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNo { get; set; }
        public int CustomerGender { get; set; } = 3;
        public string CustomerLevel { get; set; }
    }
}
