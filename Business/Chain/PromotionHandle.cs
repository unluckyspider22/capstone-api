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
        public override void Handle(OrderResponseModel order)
        {
            var promotions = order.Promotions;
            foreach (Promotion promotion in promotions)
            {
                HandleStore(promotion, order);
                HandleSalesMode(promotion, order);
                HandlePayment(promotion, order);
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
            if(!Common.CompareBinary(order.OrderDetail.PaymentMethod, promotion.PaymentMethod))
            {
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Invalid_PaymentType);
            }
        }
        private void HandleHoliday(Promotion promotion)
        {

        }
        private void HandleGender(Promotion promotion)
        {

        }
        private void CheckTimeframe(Promotion promotion)
        {

        }
        private void CheckMembership(Promotion promotion)
        {

        }

    }
}
