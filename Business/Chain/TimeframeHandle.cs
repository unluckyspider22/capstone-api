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
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ApplicationCore.Chain
{
    public interface ITimeframeHandle : IHandler<OrderResponseModel>
    {
        void SetHolidays(List<Holiday> holidays);
        void SetPromotions(List<Promotion> promotions);

    }
    public class TimeframeHandle : Handler<OrderResponseModel>, ITimeframeHandle
    {

        private List<Promotion> _promotions;
        private List<Holiday> _listPublicHoliday;
        public void SetPromotions(List<Promotion> promotions)
        {
            _promotions = promotions;
        }
        public override void Handle(OrderResponseModel order)
        {
            foreach (var promotion in _promotions)
            {
                //HandleExpiredDate(promotion, order);
                if (!promotion.ForHoliday.Equals(AppConstant.EnvVar.FOR_HOLIDAY))
                {
                    HandleHolidayAsync(order);
                }
                HandleDayOfWeek(promotion, order.CustomerOrderInfo.BookingDate.DayOfWeek);
                HandleHour(promotion, order.CustomerOrderInfo.BookingDate.Hour);
            }
            base.Handle(order);
        }

        public void HandleHolidayAsync(OrderResponseModel order)
        {
            if (_listPublicHoliday != null && _listPublicHoliday.Count() > 0)
            {
                foreach (var holiday in _listPublicHoliday)
                {
                    if (order.CustomerOrderInfo.BookingDate.Day == ((DateTime)holiday.Date).Day && order.CustomerOrderInfo.BookingDate.Month == ((DateTime)holiday.Date).Month)
                    {
                        throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_Holiday);
                    }
                }
            }
        }
        public void HandleDayOfWeek(Promotion promotion, DayOfWeek dayOfWeek)
        {
            if (!Common.CompareBinary(((int)(Math.Pow(2, (int)dayOfWeek))).ToString(), promotion.DayFilter))
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_DayInWeek);
            }
        }
        public void HandleHour(Promotion promotion, int hourOfDay)
        {
            if (!Common.CompareBinary(((int)Math.Pow(2, hourOfDay)).ToString(), promotion.HourFilter))
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_HourFrame);
            }
        }

        public void SetHolidays(List<Holiday> holidays)
        {
            _listPublicHoliday = holidays;
        }
    }
}

