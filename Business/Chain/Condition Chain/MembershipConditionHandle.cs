using ApplicationCore.Models;
using ApplicationCore.Request;
using Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Chain
{
    public interface IMembershipConditionHandle : IHandler<OrderResponseModel>
    {
        void SetConditionModel(ConditionModel conditions);
    }
    public class MembershipConditionHandle : Handler<OrderResponseModel>, IMembershipConditionHandle
    {
        private ConditionModel _condition;

        public override void Handle(OrderResponseModel order)
        {
            if (_condition is MembershipConditionModel)
            {

            }
            else
            {
                base.Handle(order);
            }
        }
        public void SetConditionModel(ConditionModel conditions)
        {
            _condition = conditions;
        }
    }
}
