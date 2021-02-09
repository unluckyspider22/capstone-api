using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System;

namespace ApplicationCore.Services
{
    public class ConditionRuleService : BaseService<ConditionRule, ConditionRuleDto>, IConditionRuleService
    {
        public ConditionRuleService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        protected override IGenericRepository<ConditionRule> _repository => _unitOfWork.ConditionRuleRepository;

        public async Task<List<ConditionRuleResponse>> ReorderResult(List<ConditionRule> paramList)
        {
            List<ConditionRuleResponse> result = new List<ConditionRuleResponse>();

            foreach (var rule in paramList)
            {
                ConditionRuleResponse conditionRuleResponse = new ConditionRuleResponse
                {
                    ConditionRuleId = rule.ConditionRuleId,
                    BrandId = rule.BrandId,
                    RuleName = rule.RuleName,
                    Description = rule.Description,
                    ConditionGroups = new List<ConditionGroupResponse>(),
                };
                ICollection<ConditionGroup> groupsParam = rule.ConditionGroup;
                foreach (var group in groupsParam)
                {
                    ConditionGroupResponse groupResponse = new ConditionGroupResponse
                    {
                        ConditionRuleId = rule.ConditionRuleId,
                        ConditionGroupId = group.ConditionGroupId,
                        GroupNo = group.GroupNo,
                        NextOperator = group.NextOperator,
                        Conditions = new List<object>(),
                    };
                    if (group.ProductCondition != null && group.ProductCondition.Count > 0)
                    {
                        var productConditions = group.ProductCondition;
                        foreach (var productCondition in productConditions)
                        {
                            groupResponse.Conditions.Add(_mapper.Map<ProductConditionDto>(productCondition));
                        }
                    }
                    if (group.OrderCondition != null && group.OrderCondition.Count > 0)
                    {
                        var orderConditions = group.OrderCondition;
                        foreach (var orderCondition in orderConditions)
                        {
                            groupResponse.Conditions.Add(_mapper.Map<OrderConditionDto>(orderCondition));
                        }
                    }
                    if (group.MembershipCondition != null && group.MembershipCondition.Count > 0)
                    {
                        var membershipConditions = group.MembershipCondition;
                        foreach (var membershipCondition in membershipConditions)
                        {
                            groupResponse.Conditions.Add(_mapper.Map<MembershipConditionDto>(membershipCondition));
                        }
                    }
                    groupResponse.Conditions = groupResponse.Conditions.OrderBy(
                        o => (o.GetType() == typeof(ProductConditionDto) ? ((ProductConditionDto)o).IndexGroup
                        : o.GetType() == typeof(OrderConditionDto) ? ((OrderConditionDto)o).IndexGroup
                        : ((MembershipConditionDto)o).IndexGroup)).ToList();
                    conditionRuleResponse.ConditionGroups.Add(groupResponse);
                }
                conditionRuleResponse.ConditionGroups = conditionRuleResponse.ConditionGroups.OrderBy(o => o.GroupNo).ToList();
                result.Add(conditionRuleResponse);
            }
            return result;
        }

        public async Task<ConditionRuleResponse> ReorderResult(ConditionRule param)
        {
            ConditionRuleResponse conditionRuleResponse = new ConditionRuleResponse
            {
                ConditionRuleId = param.ConditionRuleId,
                BrandId = param.BrandId,
                RuleName = param.RuleName,
                Description = param.Description,
                ConditionGroups = new List<ConditionGroupResponse>(),
            };
            ICollection<ConditionGroup> groupsParam = param.ConditionGroup;
            foreach (var group in groupsParam)
            {
                ConditionGroupResponse groupResponse = new ConditionGroupResponse
                {
                    ConditionRuleId = param.ConditionRuleId,
                    ConditionGroupId = group.ConditionGroupId,
                    GroupNo = group.GroupNo,
                    NextOperator = group.NextOperator,
                    Conditions = new List<object>(),
                };
                if (group.ProductCondition != null && group.ProductCondition.Count > 0)
                {
                    var productConditions = group.ProductCondition;
                    foreach (var productCondition in productConditions)
                    {
                        groupResponse.Conditions.Add(_mapper.Map<ProductConditionDto>(productCondition));
                    }
                }
                if (group.OrderCondition != null && group.OrderCondition.Count > 0)
                {
                    var orderConditions = group.OrderCondition;
                    foreach (var orderCondition in orderConditions)
                    {
                        groupResponse.Conditions.Add(_mapper.Map<OrderConditionDto>(orderCondition));
                    }
                }
                if (group.MembershipCondition != null && group.MembershipCondition.Count > 0)
                {
                    var membershipConditions = group.MembershipCondition;
                    foreach (var membershipCondition in membershipConditions)
                    {
                        groupResponse.Conditions.Add(_mapper.Map<MembershipConditionDto>(membershipCondition));
                    }
                }
                groupResponse.Conditions = groupResponse.Conditions.OrderBy(
                    o => (o.GetType() == typeof(ProductConditionDto) ? ((ProductConditionDto)o).IndexGroup
                    : o.GetType() == typeof(OrderConditionDto) ? ((OrderConditionDto)o).IndexGroup
                    : ((MembershipConditionDto)o).IndexGroup)).ToList();
                conditionRuleResponse.ConditionGroups.Add(groupResponse);
                conditionRuleResponse.ConditionGroups = conditionRuleResponse.ConditionGroups.OrderBy(o => o.GroupNo).ToList();
            }

            return conditionRuleResponse;
        }
    }
}
