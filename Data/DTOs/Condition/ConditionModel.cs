﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class ConditionModel
    {
        public Guid Id { get; set; }
        public int? Index { get; set; }
        public string NextOperator { get; set; }

        public bool IsMatch { get; set; } = true;
    }
}
