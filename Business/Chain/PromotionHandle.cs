using ApplicationCore.Request;
using ApplicationCore.Utils;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using System.Linq;
using System.Net;

namespace ApplicationCore.Chain
{
    public interface IPromotionHandle : IHandler<OrderResponseModel>
    {

    }
    public class PromotionHandle : Handler<OrderResponseModel>, IPromotionHandle
    {
        private readonly ITimeframeHandle _timeframeHandle;

        public PromotionHandle(ITimeframeHandle timeframeHandle)
        {
            _timeframeHandle = timeframeHandle;
        }

        public override void Handle(OrderResponseModel order)
        {
            var promotions = order.Promotions;
            HandleExclusive(order);
            foreach (Promotion promotion in promotions)
            {
                HandleStore(promotion, order);
                HandleSalesMode(promotion, order);
                HandlePayment(promotion, order);
                HandleGender(promotion, order);
                HandleHoliday(promotion, order);
            }
            base.Handle(order);
        }
        #region Handle Exclusive
        private void HandleExclusive(OrderResponseModel order)
        {
            var promotions = order.Promotions;
            if (promotions.Any(w => w.Exclusive.Equals(AppConstant.EnvVar.Exclusive.GlobalExclusive)) && promotions.Count() > 1)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Exclusive_Promotion);
            }
            if (promotions.Where(w => w.Exclusive.Equals(AppConstant.EnvVar.Exclusive.ClassExclusiveOrder)).Count() > 1)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Exclusive_Promotion);
            }
            if (promotions.Where(w => w.Exclusive.Equals(AppConstant.EnvVar.Exclusive.ClassExclusiveProduct)).Count() > 1)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Exclusive_Promotion);
            }
            if (promotions.Where(w => w.Exclusive.Equals(AppConstant.EnvVar.Exclusive.ClassExclusiveShipping)).Count() > 1)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Exclusive_Promotion);
            }
        }
        #endregion
        #region Handle Store
        private void HandleStore(Promotion promotion, OrderResponseModel order)
        {
            if (promotion.PromotionStoreMapping.Where(w => w.Store.StoreCode.Equals(order.OrderDetail.StoreInfo.StoreId)).Count() == 0)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_Store);
            }
        }
        #endregion
        #region Handle Sales Mode
        private void HandleSalesMode(Promotion promotion, OrderResponseModel order)
        {
            if (!Common.CompareBinary(order.OrderDetail.SalesMode, promotion.SaleMode))
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_SaleMode);
            }
        }
        #endregion
        #region Handle Payment
        private void HandlePayment(Promotion promotion, OrderResponseModel order)
        {
            if (!Common.CompareBinary(order.OrderDetail.PaymentMethod, promotion.PaymentMethod))
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_PaymentType);
            }
        }
        #endregion
        #region Handle Holiday
        private void HandleHoliday(Promotion promotion, OrderResponseModel order)
        {
            if (promotion.ForHoliday.Equals(AppConstant.EnvVar.FOR_WEEKEND))
            {
                CheckTimeframe(promotion, order, isWeekend: true);
            }
            if (promotion.ForHoliday.Equals(AppConstant.EnvVar.FOR_HOLIDAY))
            {
                CheckTimeframe(promotion, order, isHoliday: true);
            }
            if (!promotion.DayFilter.Equals(AppConstant.EnvVar.NO_FILTER))
            {
                _timeframeHandle.Handle(order);
            }
            if (!promotion.HourFilter.Equals(AppConstant.EnvVar.NO_FILTER))
            {
                _timeframeHandle.Handle(order);
            }
        }
        #endregion
        #region Handle Gender
        private void HandleGender(Promotion promotion, OrderResponseModel order)
        {
            if (!promotion.Gender.Equals(order.Customer.CustomerGender))
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_Gender);
            }
        }
        #endregion
        #region Check timeframe
        private void CheckTimeframe(Promotion promotion, OrderResponseModel order, bool isWeekend = false, bool isHoliday = false)
        {
            //Kiểm tra promotion không áp dụng cho các ngày cuối tuần
            if (isWeekend && !isHoliday)
            {
                var bookingDayOfWeek = order.OrderDetail.BookingDate.DayOfWeek;
                if ((int)bookingDayOfWeek == AppConstant.EnvVar.Holiday.FRIDAY
                    || (int)bookingDayOfWeek == AppConstant.EnvVar.Holiday.SATURDAY
                    || (int)bookingDayOfWeek == AppConstant.EnvVar.Holiday.SUNDAY)
                {
                    throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_TimeFrame);
                }
            }
            //Kiểm tra promotion không áp dụng vào các ngày lễ
            /*else
            {
                //Kiểm tra promotion có cho member hay không? Có thì handle member
                if (promotion.ForMembership.Equals(AppConstant.ENVIRONMENT_VARIABLE.FOR_MEMBER))
                {
                    //ApplyHandle => PromotionHandle => TimeframeHandle => MembershipHandle => OrderHandle => ProductHandle
                    _timeframeHandle.SetNext(_membershipHandle).SetNext(_orderHandle);
                }
                else
                {
                    _timeframeHandle.SetNext(_orderHandle).SetNext(_productHandle);
                }
            }*/

        }
        #endregion

    }
}
