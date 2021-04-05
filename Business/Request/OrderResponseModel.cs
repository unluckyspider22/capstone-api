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
        public Guid? PromotionTierId { get; set; }
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
        [StringLength(1000)]
        public string Note { get; set; }
        public int PaymentMethod { get; set; }
        public StoreInfo StoreInfo { get; set; }
    }
    public class Gift
    {
        [StringLength(20)]

        public string ProductCode { get; set; }

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
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountFromOrder { get; set; }
        public decimal FinalAmount { get; set; }
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
