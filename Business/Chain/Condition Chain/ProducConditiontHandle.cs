using ApplicationCore.Models;
using ApplicationCore.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Chain
{
    public interface IProductConditionHandle : IHandler<OrderResponseModel>
    {
        void SetConditionModel(ConditionModel condition);
    }
    public class ProducConditiontHandle : Handler<OrderResponseModel>, IProductConditionHandle
    {
        private ConditionModel _condition;

        public override void Handle(OrderResponseModel order)
        {
            if (_condition is ProductConditionModel)
            {

            }
            else
            {
                base.Handle(order);
            }
        }
        public void SetConditionModel(ConditionModel condition)
        {
            _condition = condition;
        }
        
    }
}
