﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class AvailableDto
    {
        public Guid PromotionTierId { get; set; }
        public string RuleName { get; set; }
        public string Description { get; set; }
        public string ActionType { get; set; }
        public string DiscountType { get; set; }
    }
}
