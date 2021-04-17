using ApplicationCore.Models;
using ApplicationCore.Request;
using ApplicationCore.Utils;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Chain
{
    public interface IOrderConditionHandle : IHandler<Order>
    {
        void SetConditionModel(ConditionModel conditions);

    }
    public class OrderConditionHandle : Handler<Order>, IOrderConditionHandle
    {
        private ConditionModel _condition;

        public override void Handle(Order order)
        {
            if (_condition is OrderConditionModel)
            {
                HandleMinAmount((OrderConditionModel)_condition, order);
                
                HandleMinQuantity((OrderConditionModel)_condition, order);

                //throw new ErrorObj(code: 400, message: " orderCondition.IsMatch : " + _condition.IsMatch);
            }
            else
            {
                base.Handle(order);
            }
        }

        private void HandleMinAmount(OrderConditionModel orderCondition, Order order)
        {
            /*throw new ErrorObj(code: 400, message:"Compare: "+ Common.Compare<decimal>(orderCondition.AmountOperator, order.OrderDetail.Amount, orderCondition.Amount));*/
            if (!Common.Compare<decimal>(orderCondition.AmountOperator, (decimal)order.FinalAmount, orderCondition.Amount))
            {
                orderCondition.IsMatch = false;
            }
        }
        private void HandleMinQuantity(OrderConditionModel orderCondition, Order order)
        {
            if (!Common.Compare<decimal>(orderCondition.QuantityOperator, order.CustomerOrderInfo.CartItems.Count(), orderCondition.Quantity))
            {
                orderCondition.IsMatch = false;
            }
        }
        public void SetConditionModel(ConditionModel conditions)
        {
            _condition = conditions;
        }
    }


}
