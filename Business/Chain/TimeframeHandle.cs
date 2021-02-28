using ApplicationCore.Request;
using ApplicationCore.Utils;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using System.IO;
using System.Linq;
using System.Net;
using System;
using ApplicationCore.Services;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using AutoMapper;

namespace ApplicationCore.Chain
{
    public interface ITimeframeHandle : IHandler<OrderResponseModel>
    {

    }
    public class TimeframeHandle : Handler<OrderResponseModel>, ITimeframeHandle
    {
     
    
        private readonly IHolidayService _holidayService;

        public TimeframeHandle(IHolidayService holidayService)
        {
            _holidayService = holidayService;
        }

        public override void Handle(OrderResponseModel order)
        {
            var promotions = order.Promotions;
            foreach (Promotion promotion in promotions)
            {
                //HandleExpiredDate(promotion, order);
                if (!promotion.ForHoliday.Equals(AppConstant.EnvVar.FOR_HOLIDAY))
                { HandleHoliday(promotion, order); }
                HandleDayOfWeek(promotion, order.OrderDetail.BookingDate.DayOfWeek);
                HandleHour(promotion, order.OrderDetail.BookingDate.Hour);
            }
            base.Handle(order);
        }
     
        public void HandleHoliday(Promotion promotion, OrderResponseModel order)
        { 
                var listPublicHoliday = _holidayService.GetHolidays();
                foreach (var holiday in listPublicHoliday)
                {
                if (order.OrderDetail.BookingDate.Day == holiday.Date.Day && order.OrderDetail.BookingDate.Month == holiday.Date.Month)
                { 
                    throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_Holiday); 
                }
                //    if (order.OrderDetail.BookingDate.Date.ToString("dd/MM/yyyy").Equals(holiday.Date.Date.ToString("dd/MM/yyyy")))
                //{
                //}
            }

        }
        public void HandleDayOfWeek(Promotion promotion, DayOfWeek dayOfWeekStr)
        {
            int dayOfWeekNum = 0;
            switch (dayOfWeekStr.ToString())
            {
                case "Monday": dayOfWeekNum = 1; break;
                case "Tuesday": dayOfWeekNum = 2; break;
                case "Wednesday": dayOfWeekNum = 4; break;
                case "Thursday": dayOfWeekNum = 8; break;
                case "Friday": dayOfWeekNum = 16; break;
                case "Saturday": dayOfWeekNum = 32; break;
                case "Sunday": dayOfWeekNum = 64; break;
            }
            if (!Common.CompareBinaryForDay(dayOfWeekNum, promotion.DayFilter))
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_DayInWeek);
            }
        }
        public void HandleHour(Promotion promotion, int hourOfDay)
        {
            if (!Common.CompareBinaryForHour(hourOfDay, promotion.HourFilter))
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_HourFrame);
            }
        }
    }
}

