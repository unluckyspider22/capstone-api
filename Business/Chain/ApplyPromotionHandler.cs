using ApplicationCore.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Chain
{
    public interface IApplyPromotionHandler : IHandler<OrderResponseModel>
    {

    }
    public class ApplyPromotionHandler : Handler<OrderResponseModel>, IApplyPromotionHandler
    {
        private readonly IPromotionHandle _promotionHandle;


        public ApplyPromotionHandler(IPromotionHandle promotionHandle)
        {
            _promotionHandle = promotionHandle;
        }

        public override void Handle(OrderResponseModel order)
        {
            #region Check condition
            Setorder(order);
            /*_promotionHandle.SetNext(_orderHandle).SetNext(_productHandle);*/
            _promotionHandle.Handle(order);
            #endregion
            base.Handle(order);
        }

        private void Setorder(OrderResponseModel order)
        {
            order.Discount ??= 0;
            order.DiscountOrderDetail ??= 0;
            order.TotalAmount ??= 0;
            order.FinalAmount ??= 0;
        }
    }
}
