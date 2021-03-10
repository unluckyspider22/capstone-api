using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class ProductConditionModel : ConditionModel
    {
        public string ProductConditionType { get; set; }
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public decimal ProductQuantity { get; set; }
        public string QuantityOperator { get; set; }
    }
}
