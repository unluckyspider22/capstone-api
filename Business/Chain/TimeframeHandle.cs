using ApplicationCore.Request;
using Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Chain
{
    public interface ITimeframeHandle : IHandler<OrderResponseModel>
    {

    }
    public class TimeframeHandle : Handler<OrderResponseModel>, ITimeframeHandle
    {
        public override void Handle(OrderResponseModel order)
        {
            var promotions = order.Promotions;

            base.Handle(order);
        }
    }
}
