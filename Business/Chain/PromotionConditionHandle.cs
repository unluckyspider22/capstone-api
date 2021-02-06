using ApplicationCore.Request;
using ApplicationCore.Utils;
using Infrastructure.DTOs;
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
    public interface IPromotionConditionHandle : IHandler<OrderResponseModel>
    {

    }
    public class PromotionConditionHandle : Handler<OrderResponseModel>, IPromotionConditionHandle
    {
        public override void Handle(OrderResponseModel order)
        {
            foreach (var promotion in order.Promotions)
            {
                int invalidPromotionDetails = 0;
                foreach (var promotionTier in promotion.PromotionTier)
                {
                    #region Handle Condition
                    var handle = HandleCondition(promotionTier, order);
                    #endregion
                    if(handle > 0)
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
        private int HandleCondition(PromotionTier promotionTier, OrderResponseModel order)
        {
            int invalidPromotionDetails = 0;

            foreach (var conditionGroup in promotionTier.ConditionRule.ConditionGroup)
            {
                
            }
            return invalidPromotionDetails;
        }

        private void HandleOrderCondition()
        {
          /*  var errMsg = new List<string>();
            foreach (var orderCondition in conditionGroup.OrderCondition)
            {
                #region Handle order condition
                string messageHandleMinAmount = HandleMinAmount(orderCondition, order);
                if (!string.IsNullOrEmpty(messageHandleMinAmount))
                {
                    errMsg.Add(messageHandleMinAmount);
                }
                string messageHandleMinQuantity = HandleMinQuantity(orderCondition, order);
                if (!string.IsNullOrEmpty(messageHandleMinQuantity))
                {
                    errMsg.Add(messageHandleMinQuantity);
                }
                #endregion
            }
            if (errMsg.Count() > 0)
            {
                invalidPromotionDetails++;

            }*/
        }

        private string HandleMinAmount(OrderCondition orderCondition, OrderResponseModel order)
        {
            /*throw new ErrorObj(code: 400, message:"Compare: "+ Common.Compare<decimal>(orderCondition.AmountOperator, order.OrderDetail.Amount, orderCondition.Amount));*/
            if (!Common.Compare<decimal>(orderCondition.AmountOperator, order.OrderDetail.Amount, orderCondition.Amount))
            {
                return AppConstant.ErrMessage.Invalid_MinAmount;
            }

            return string.Empty;
        }
        private string HandleMinQuantity(OrderCondition orderCondition, OrderResponseModel order)
        {
            if (!Common.Compare<decimal>(orderCondition.QuantityOperator, order.OrderDetail.OrderDetailResponses.Count(), orderCondition.Quantity))
            {
                return AppConstant.ErrMessage.Invalid_Min_Quantity;
            }

            return string.Empty;
        }
    }
}
