using ApplicationCore.Request;
using Infrastructure.DTOs;
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
        private readonly IConditionHandle _conditionHandle;
        private readonly IApplyPromotion _applyPromotion;

        public ApplyPromotionHandler(IPromotionHandle promotionHandle, IConditionHandle conditionHandle, IApplyPromotion applyPromotion)
        {
            _promotionHandle = promotionHandle;
            _conditionHandle = conditionHandle;
            _applyPromotion = applyPromotion;
        }

        public override void Handle(OrderResponseModel order)
        {
            #region Check condition
            Setorder(order);
            //Thứ tự là:
            //ApplyHandle => PromotionHandle => TimeframeHandle(nếu có) => ConditionHandle
            _promotionHandle.SetNext(_conditionHandle);
            _promotionHandle.Handle(order);
            #endregion
            #region Apply action
            _applyPromotion.Apply(order);
            #endregion
            /*base.Handle(order);*/
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
