﻿using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class ProductConditionMapping
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductConditionId { get; set; }
        public DateTime InsDate { get; set; } = DateTime.Now;
        public DateTime UpdTime { get; set; } = DateTime.Now;

        public virtual Product Product { get; set; }
        public virtual ProductCondition ProductCondition { get; set; }
    }
}
