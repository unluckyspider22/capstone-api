﻿
using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IConditionRuleService : IBaseService<ConditionRule, ConditionRuleDto>
    {
        public Task<List<ConditionRuleResponse>> ReorderResult(List<ConditionRule> paramList);
        public Task<ConditionRuleResponse> ReorderResult(ConditionRule param);
    }
}
