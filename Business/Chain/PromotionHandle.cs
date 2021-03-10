using ApplicationCore.Request;
using ApplicationCore.Utils;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ApplicationCore.Chain
{
    public interface IPromotionHandle : IHandler<OrderResponseModel>
    {
        void SetPromotions(List<Promotion> promotions);
    }
    public class PromotionHandle : Handler<OrderResponseModel>, IPromotionHandle
    {
        private List<Promotion> _promotions;

        public void SetPromotions(List<Promotion> promotions)
        {
            _promotions = promotions;
        }
        public override void Handle(OrderResponseModel order)
        {
            HandleExclusive();
            foreach (Promotion promotion in _promotions)
            {
                HandleStore(promotion, order);
                HandleSalesMode(promotion, order);
                HandlePayment(promotion, order);
                HandleGender(promotion, order);
            }
            base.Handle(order);
        }
        #region Handle Exclusive
        private void HandleExclusive()
        {

            if (_promotions.Any(w => w.Exclusive.Equals(AppConstant.EnvVar.Exclusive.GlobalExclusive)) && _promotions.Count() > 1)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Exclusive_Promotion);
            }
            if (_promotions.Where(w => w.Exclusive.Equals(AppConstant.EnvVar.Exclusive.ClassExclusiveOrder)).Count() > 1)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Exclusive_Promotion);
            }
            if (_promotions.Where(w => w.Exclusive.Equals(AppConstant.EnvVar.Exclusive.ClassExclusiveProduct)).Count() > 1)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Exclusive_Promotion);
            }
            if (_promotions.Where(w => w.Exclusive.Equals(AppConstant.EnvVar.Exclusive.ClassExclusiveShipping)).Count() > 1)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Exclusive_Promotion);
            }
        }
        #endregion
        #region Handle Store
        private void HandleStore(Promotion promotion, OrderResponseModel order)
        {
            if (promotion.PromotionStoreMapping.Where(w => w.Store.StoreCode.Equals(order.CustomerOrderInfo.Attributes.StoreInfo.StoreId)).Count() == 0)
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_Store);
            }
        }
        #endregion
        #region Handle Sales Mode
        private void HandleSalesMode(Promotion promotion, OrderResponseModel order)
        {
            if (!Common.CompareBinary(order.CustomerOrderInfo.Attributes.SalesMode, promotion.SaleMode))
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_SaleMode);
            }
        }
        #endregion
        #region Handle Payment
        private void HandlePayment(Promotion promotion, OrderResponseModel order)
        {
            if (!Common.CompareBinary(order.CustomerOrderInfo.Attributes.PaymentMethod, promotion.PaymentMethod))
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_PaymentType);
            }
        }
        #endregion

        #region Handle Gender
        private void HandleGender(Promotion promotion, OrderResponseModel order)
        {
            if (!Common.CompareBinary(order.CustomerOrderInfo.Customer.CustomerGender, promotion.Gender))
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_Gender);
            }
        }
        #endregion
    }
}
