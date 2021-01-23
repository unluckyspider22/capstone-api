using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class OrderParam
    {
        public double totalPrice { get; set; }
        public List<OrderItem> items { get; set; }
        public int quantity { get; set; }
    }
    public class OrderItem {
        public string cateCode { get; set; }
        public string comboCode { get; set; }
        public string productCode { get; set; }
        public string size { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public string  name { get; set; }
        public string tag { get; set; }

    }
}
