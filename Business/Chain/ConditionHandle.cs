using ApplicationCore.Models;
using ApplicationCore.Request;
using ApplicationCore.Utils;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.DTOs.Condition;
using Infrastructure.Helper;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApplicationCore.Chain
{
    public interface IConditionHandle : IHandler<OrderResponseModel>
    {

    }
    public class ConditionHandle : Handler<OrderResponseModel>, IConditionHandle
    {
        private readonly IOrderConditionHandle _orderConditionHandle;
        private readonly IProductConditionHandle _productConditionHandle;
        private readonly IMembershipConditionHandle _membershipConditionHandle;
        private readonly IMapper _mapper;

        public ConditionHandle(IOrderConditionHandle orderConditionHandle, IProductConditionHandle productConditionHandle, IMembershipConditionHandle membershipConditionHandle, IMapper mapper)
        {
            _orderConditionHandle = orderConditionHandle;
            _productConditionHandle = productConditionHandle;
            _membershipConditionHandle = membershipConditionHandle;
            _mapper = mapper;
        }

        public override void Handle(OrderResponseModel order)
        {
            foreach (var promotion in order.Promotions)
            {
                int invalidPromotionDetails = 0;
                foreach (var promotionTier in promotion.PromotionTier)
                {
                    #region Handle Condition
                    var handle = HandleConditionGroup(promotionTier, order);
                    #endregion
                    if (handle > 0)
                    {
                        invalidPromotionDetails++;
                        continue;
                    }
                    order.PromotionTierIds.Add(promotionTier.PromotionTierId);

                }
                if (invalidPromotionDetails == promotion.PromotionTier.Count && invalidPromotionDetails > 0)
                {
                    throw new ErrorObj((int)HttpStatusCode.BadRequest, AppConstant.ErrMessage.NotMatchCondition);
                }
            }
            base.Handle(order);
        }
        private int HandleConditionGroup(PromotionTier promotionTier, OrderResponseModel order)
        {
            int invalidPromotionDetails = 0;
            var conditionGroupModels = new List<ConditionGroupModel>();
            foreach (var conditionGroup in promotionTier.ConditionRule.ConditionGroup)
            {
                var conditions = InitConditionModel(conditionGroup);
                if (conditions != null)
                {
                    foreach (var condition in conditions)
                    {
                        #region Handle cho từng condition dựa vào loại của nó
                        //Tạo chuỗi handle cho từng loại condition
                        _orderConditionHandle.SetNext(_productConditionHandle).SetNext(_membershipConditionHandle);
                        _orderConditionHandle.SetConditionModel(condition);
                        _productConditionHandle.SetConditionModel(condition);
                        _membershipConditionHandle.SetConditionModel(condition);
                        _orderConditionHandle.Handle(order);
                        #endregion
                    }
                    var conditionResult = CompareConditionInGroup(conditions);
                    var groupModel = new ConditionGroupModel(conditionGroup.GroupNo, conditionGroup.NextOperator, conditionResult);
                    conditionGroupModels.Add(groupModel);
                }
            }
            var result = CompareConditionGroup(conditionGroupModels);
            if (!result)
            {
                invalidPromotionDetails++;
            }
            return invalidPromotionDetails;
        }

        private bool CompareConditionGroup(List<ConditionGroupModel> conditionGroups)
        {
            conditionGroups = conditionGroups.OrderBy(el => el.GroupNo).ToList();
            bool result = conditionGroups.First().IsMatch;
            foreach (var condition in conditionGroups)
            {
                if (conditionGroups.Count() == 1)
                {
                    return condition.IsMatch;
                }
                else
                {
                    int index = (int)condition.GroupNo;
                    if (index != conditionGroups.Count() - 1)
                    {
                        if (!string.IsNullOrEmpty(condition.NextOperator))
                        {
                            int nextIndex = index + 1;
                            if (condition.NextOperator.Equals(AppConstant.Operator.AND))
                            {
                                result = result && conditionGroups[nextIndex].IsMatch;

                            }
                            else
                            if (condition.NextOperator.Equals(AppConstant.Operator.OR))
                            {
                                result = result || conditionGroups[nextIndex].IsMatch;
                            }

                        }
                    }

                }
            }
            return result;
        }

        /*  private List<ConditionGroupModel> InitConditionGroupModel(ConditionRule conditionRule)
          {
              List<ConditionGroupModel> conditionGroups = new List<ConditionGroupModel>();
              foreach()

              return conditionGroups;
          }*/

        private bool CompareConditionInGroup(List<ConditionModel> conditions)
        {
            conditions = conditions.OrderBy(el => el.Index).ToList();
            bool result = conditions.First().IsMatch;
            foreach (var condition in conditions)
            {
                if (conditions.Count() == 1)
                {
                    return condition.IsMatch;
                }
                else
                {
                    int index = (int)condition.Index;
                    if (index != conditions.Count() - 1)
                    {
                        if (!string.IsNullOrEmpty(condition.NextOperator))
                        {
                            int nextIndex = index + 1;
                            if (condition.NextOperator.Equals(AppConstant.Operator.AND))
                            {
                                result = result && conditions[nextIndex].IsMatch;

                            }
                            else
                            if (condition.NextOperator.Equals(AppConstant.Operator.OR))
                            {
                                result = result || conditions[nextIndex].IsMatch;
                            }

                        }
                    }

                }
            }
            return result;
        }

        #region Tạo 1 list condition
        private List<ConditionModel> InitConditionModel(ConditionGroup conditionGroup)
        {
            List<ConditionModel> conditionModels = new List<ConditionModel>();

            foreach (var orderCondition in conditionGroup.OrderCondition)
            {
                var entity = _mapper.Map<OrderConditionModel>(orderCondition);
                entity.Id = orderCondition.OrderConditionId;
                entity.Index = orderCondition.IndexGroup;
                entity.NextOperator = orderCondition.NextOperator;
                conditionModels.Add(entity);
            }
            foreach (var productCondition in conditionGroup.ProductCondition)
            {
                var entity = _mapper.Map<ProductConditionModel>(productCondition);
                entity.Id = productCondition.ProductConditionId;
                entity.Index = productCondition.IndexGroup;
                entity.NextOperator = productCondition.NextOperator;
                conditionModels.Add(entity);
            }
            foreach (var membershipCondition in conditionGroup.MembershipCondition)
            {
                var entity = _mapper.Map<ProductConditionModel>(membershipCondition);
                entity.Id = membershipCondition.MembershipConditionId;
                entity.Index = membershipCondition.IndexGroup;
                entity.NextOperator = membershipCondition.NextOperator;
                conditionModels.Add(entity);
            }
            conditionModels = conditionModels.OrderBy(el => el.Index).ToList();
            return conditionModels;
        }
        #endregion


    }
}
