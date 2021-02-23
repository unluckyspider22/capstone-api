using ApplicationCore.Request;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public void HandleExpiredDate(Promotion promotion, OrderResponseModel order)
        {
            if(order.OrderDetail.BookingDate.CompareTo(promotion.StartDate)<0 && order.OrderDetail.BookingDate.CompareTo(promotion.EndDate) > 0)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_Time);
            }
        }
    }
}
