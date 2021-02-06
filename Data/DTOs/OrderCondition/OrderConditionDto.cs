using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class OrderConditionDto
    {
        public Guid OrderConditionId { get; set; }
        [StringLength(1)]
        public string NextOperator { get; set; }
        [Range(0,999999)]
        public decimal Quantity { get; set; }
        [StringLength(1)]
        public string QuantityOperator { get; set; }
        [Range(0, 9999999999)]
        public decimal Amount { get; set; }
        [StringLength(1)]
        public string AmountOperator { get; set; }
        [Range(0, 999)]
        public int IndexGroup { get; set; }
    }
}
