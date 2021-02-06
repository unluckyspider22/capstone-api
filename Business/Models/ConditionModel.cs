﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class ConditionResponseModel
    {
        public List<ConditionModel> ConditionModels { get; set; }
    }
    public class ConditionModel
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public string NextOperator { get; set; }
    }
}
