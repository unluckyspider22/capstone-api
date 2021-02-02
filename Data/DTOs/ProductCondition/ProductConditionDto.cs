using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class ProductConditionDto
    {
        [StringLength(1)]
        public string ProductConditionType { get; set; }
        [StringLength(1)]
        public string ProductType { get; set; }
        [StringLength(50)]
        public string ProductCode { get; set; }
        [StringLength(100)]
        public string ProductName { get; set; }
        [Range(0,999999)]
        public decimal ProductQuantity { get; set; }
        [StringLength(1)]
        public string QuantityOperator { get; set; }
        [StringLength(50)]
        public string ParentCode { get; set; }
        [StringLength(1)]
        public string NextOperator { get; set; }
    }
}
