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
            Promotions = new List<Promotion>();
            Gift = new List<Gift>();
            Customer = new CustomerInfo();

        }

        public OrderInfoModel OrderDetail { get; set; }

        public List<Promotion> Promotions { get; set; }

        public List<VoucherResponseModel> Vouchers { get; set; }
        public List<Gift> Gift { get; set; }
        public CustomerInfo Customer { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountOrderDetail { get; set; }
        public decimal? FinalAmount { get; set; }

    }

    public class OrderInfoModel
    {
        public OrderInfoModel()
        {

            OrderDetailResponses = new List<OrderDetailResponseModel>();
        }
        public int? Id { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public string SalesMode { get; set; }
        public string Note { get; set; }
        public string PaymentMethod { get; set; }

        public decimal Amount { get; set; }

        public StoreInfoModel StoreInfo { get; set; }

        public List<OrderDetailResponseModel> OrderDetailResponses { get; set; }


    }
    public class Gift
    {
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
    }
    public class VoucherResponseModel
    {
        public string PromotionCode { get; set; }
        public string VoucherCode { get; set; }
    }
    public class StoreInfoModel
    {
        public string StoreId { get; set; }
        public string StoreName { get; set; }
        public Guid BrandId { get; set; }
        
        public string Applier { get; set; }
        public string IpAddress { get; set; }
    }
    public class OrderDetailResponseModel
    {
        public string ProductCode { get; set; }
        public string CategoryCode { get; set; }
        public string Tag { get; set; }
        public string ParentCode { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalAmount { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public string PromotionCode { get; set; }

    }
    public class OrderDetailActionModel
    {
        public Guid ActionId { get; set; }
        public Guid PromotionId { get; set; }
        public string ActionDescription { get; set; }
        public string PromotionDescription { get; set; }
        public string VoucherCode { get; set; }
        public string PromotionCode { get; set; }
        public decimal? DiscountAmount { get; set; }
    }
    public class CustomerInfo
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNo { get; set; }
        public string CustomerGender { get; set; }
    }
}
