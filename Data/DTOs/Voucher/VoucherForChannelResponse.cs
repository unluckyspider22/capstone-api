using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs.Voucher
{
    public class VoucherForChannelResponse
    {
        public VoucherForChannelResponse()
        {
            StoresData = new List<string>();
            Vouchers = new List<string>();
        }
        public PromotionInfomation PromotionData { get; set; }
        public string StoreAppied { get; set; }
        public List<string> StoresData { get; set; }
        public List<string> Vouchers { get; set; }

    }
    public class PromotionInfomation
    {
        public Guid PromotionId { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionName { get; set; }
        public string VoucherName { get; set; }
        public string PromotionType { get; set; }
        public string Exclusive { get; set; }
        public string ApplyBy { get; set; }
        public string SaleMode { get; set; }
        public string Gender { get; set; }
        public string PaymentMethod { get; set; }
        public string ForHoliday { get; set; }
        public string DayFilter { get; set; }
        public string HourFilter { get; set; }
        public string ImgUrl { get; set; }
    }
}
