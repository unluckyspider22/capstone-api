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
        private readonly ITimeframeHandle _timeframeHandle;
        private readonly IMembershipConditionHandle _membershipHandle;
        private readonly IConditionHandle _conditionHandle;
        private readonly IProductConditionHandle _productHandle;

        public ApplyPromotionHandler(IPromotionHandle promotionHandle, ITimeframeHandle timeframeHandle, IMembershipConditionHandle membershipHandle, IConditionHandle conditionHandle, IProductConditionHandle productHandle)
        {
            _promotionHandle = promotionHandle;
            _timeframeHandle = timeframeHandle;
            _membershipHandle = membershipHandle;
            _conditionHandle = conditionHandle;
            _productHandle = productHandle;
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
