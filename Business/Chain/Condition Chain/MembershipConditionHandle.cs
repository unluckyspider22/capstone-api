using ApplicationCore.Models;
using ApplicationCore.Request;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                var customer = order.Customer;
                HandleMembershipLevel((MembershipConditionModel)_condition, customer);
            }
            else
            {
                base.Handle(order);
            }
        }

        public void HandleMembershipLevel(MembershipConditionModel membershipCondition, CustomerInfo customer)
        {
            if (!string.IsNullOrEmpty(customer.CustomerLevel))
            {
                if (!membershipCondition.MembershipLevel.Contains(customer.CustomerLevel))
                {
                    membershipCondition.IsMatch = false;
                }
            }
            else membershipCondition.IsMatch = false;
        }
        public void SetConditionModel(ConditionModel conditions)
        {
            _condition = conditions;
        }
    }
}
