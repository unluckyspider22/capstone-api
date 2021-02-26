using ApplicationCore.Request;
using ApplicationCore.Utils;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Nager.Date;
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
            foreach (Promotion promotion in promotions) {
                HandleExpiredDate(promotion, order);
                HandleDayOfWeek(promotion, order.OrderDetail.BookingDate.DayOfWeek);
                HandleHour(promotion, order.OrderDetail.BookingDate.Hour);
            }
            base.Handle(order);
        }
        public void HandleExpiredDate(Promotion promotion, OrderResponseModel order)
        {
            if(order.OrderDetail.BookingDate.CompareTo(promotion.StartDate)<0 && order.OrderDetail.BookingDate.CompareTo(promotion.EndDate) > 0)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_TimeFrame);
            }
        }
        public void HandleHoliday(Promotion promotion, OrderResponseModel order)
        {
            var publicHolidays = DateSystem.GetPublicHoliday(2018, CountryCode.VN);

        }
        public void HandleDayOfWeek(Promotion promotion, DayOfWeek dayOfWeekStr) {
            int dayOfWeekNum = 0;
            switch (dayOfWeekStr.ToString()) {
                case "Monday": dayOfWeekNum = 1; break;
                case "Tuesday": dayOfWeekNum = 2; break;
                case "Wednesday": dayOfWeekNum = 4; break;
                case "Thursday": dayOfWeekNum = 8; break;
                case "Friday": dayOfWeekNum = 16; break;
                case "Saturday": dayOfWeekNum = 32; break;
                case "Sunday": dayOfWeekNum = 64; break;
            }
            if (!Common.CompareBinaryForDayHour(dayOfWeekNum, promotion.DayFilter)) {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_TimeFrame);
            }
        }
        public void HandleHour(Promotion promotion, int hourOfDay)
        {
            if (!Common.CompareBinaryForDayHour(hourOfDay, promotion.HourFilter))
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_TimeFrame);
            }
        }
    }
}
