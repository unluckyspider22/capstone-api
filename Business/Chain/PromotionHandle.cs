using ApplicationCore.Request;
using ApplicationCore.Utils;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApplicationCore.Chain
{
    public interface IPromotionHandle : IHandler<OrderResponseModel>
    {

    }
    public class PromotionHandle : Handler<OrderResponseModel>, IPromotionHandle
    {
        private readonly IPromotionHandle _promotionHandle;
        private readonly ITimeframeHandle _timeframeHandle;
        private readonly IMembershipHandle _membershipHandle;
        private readonly IOrderHandle _orderHandle;
        private readonly IProductHandle _productHandle;

        public PromotionHandle(IPromotionHandle promotionHandle, ITimeframeHandle timeframeHandle, IMembershipHandle membershipHandle, IOrderHandle orderHandle, IProductHandle productHandle)
        {
            _promotionHandle = promotionHandle;
            _timeframeHandle = timeframeHandle;
            _membershipHandle = membershipHandle;
            _orderHandle = orderHandle;
            _productHandle = productHandle;
        }

        public override void Handle(OrderResponseModel order)
        {
            var promotions = order.Promotions;
            if(promotions.Any(w => w.Exclusive.Equals(AppConstant.ENVIRONMENT_VARIABLE.EXCLULSIVE.ClassExclusiveOrder) 
                || w.Exclusive.Equals(AppConstant.ENVIRONMENT_VARIABLE.EXCLULSIVE.ClassExclusiveProduct) 
                || w.Exclusive.Equals(AppConstant.ENVIRONMENT_VARIABLE.EXCLULSIVE.ClassExclusiveShipping)) && promotions.Count() > 1)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Exclusive_Promotion);
            }
            foreach (Promotion promotion in promotions)
            {
                HandleStore(promotion, order);
                HandleSalesMode(promotion, order);
                HandlePayment(promotion, order);
                HandleGender(promotion, order);
                HandleHoliday(promotion, order);
                CheckMembership();
            }
            base.Handle(order);
        }
        private void HandleStore(Promotion promotion, OrderResponseModel order)
        {
            if (promotion.PromotionStoreMapping.Where(w => w.Store.StoreCode.Equals(order.OrderDetail.StoreInfo.StoreId)).Count() == 0)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_Store);
            }
        }

        private void HandleSalesMode(Promotion promotion, OrderResponseModel order)
        {
            if (!Common.CompareBinary(order.OrderDetail.SalesMode, promotion.SaleMode))
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_SaleMode);
            }
        }
        private void HandlePayment(Promotion promotion, OrderResponseModel order)
        {
            if (!Common.CompareBinary(order.OrderDetail.PaymentMethod, promotion.PaymentMethod))
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_PaymentType);
            }
        }
        private void HandleHoliday(Promotion promotion, OrderResponseModel order)
        {
            if (promotion.ForHoliday.Equals(AppConstant.ENVIRONMENT_VARIABLE.FOR_WEEKEND))
            {
                CheckTimeframe(promotion, order, weekend: true);
            }
            if (!promotion.DayFilter.Equals(AppConstant.ENVIRONMENT_VARIABLE.NO_FILTER))
            {
                CheckTimeframe(promotion, order);
            }
            if (!promotion.HourFilter.Equals(AppConstant.ENVIRONMENT_VARIABLE.NO_FILTER))
            {
                CheckTimeframe(promotion, order);
            }
        }
        private void HandleGender(Promotion promotion, OrderResponseModel order)
        {
            if (!promotion.Gender.Equals(order.Customer.CustomerGender))
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_Gender);
            }
        }
        private void CheckTimeframe(Promotion promotion, OrderResponseModel order, bool weekend = false,bool holiday= false)
        {
            //Check weekend
            if (weekend && !holiday)
            {
                var bookingDayOfWeek = order.OrderDetail.BookingDate.DayOfWeek;
                if ((int)bookingDayOfWeek == AppConstant.ENVIRONMENT_VARIABLE.HOLIDAY.FRIDAY
                    || (int)bookingDayOfWeek == AppConstant.ENVIRONMENT_VARIABLE.HOLIDAY.SATURDAY
                    || (int)bookingDayOfWeek == AppConstant.ENVIRONMENT_VARIABLE.HOLIDAY.SUNDAY)
                {
                    throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_TimeFrame);
                }
            }
            else { 
                _promotionHandle.SetNext(_timeframeHandle);
                if (promotion.ForMembership.Equals(AppConstant.ENVIRONMENT_VARIABLE.FOR_MEMBER))
                {
                    _timeframeHandle.SetNext(_membershipHandle).SetNext(_orderHandle).SetNext(_productHandle);
                } else
                {
                    _timeframeHandle.SetNext(_orderHandle).SetNext(_productHandle);
                }
            }

        }
        private void CheckMembership()
        {
            _promotionHandle.SetNext(_membershipHandle).SetNext(_orderHandle).SetNext(_productHandle);
        }

    }
}
