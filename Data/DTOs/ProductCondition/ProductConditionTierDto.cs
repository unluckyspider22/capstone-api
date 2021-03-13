﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class ProductConditionTierDto
    {
        public Guid ProductConditionId { get; set; }
        [StringLength(1)]
        public string ProductConditionType { get; set; }
        [Range(0, 999999)]
        public decimal ProductQuantity { get; set; }
        [StringLength(1)]
        public string QuantityOperator { get; set; }
        [StringLength(1)]
        public string NextOperator { get; set; }
        [Range(0, 999)]
        public int IndexGroup { get; set; }

        public List<ProductDto> ListProduct { get; set; }
    }
}
