using ApplicationCore.Request;
using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Chain
{
    public interface IApplyPromotionHandler : IHandler<OrderResponseModel>
    {
        void SetPromotions(List<Promotion> promotions);
        List<Promotion> GetPromotions();
    }
    public class ApplyPromotionHandler : Handler<OrderResponseModel>, IApplyPromotionHandler
    {
        private readonly IPromotionHandle _promotionHandle;
        private readonly IConditionHandle _conditionHandle;
        private readonly IApplyPromotion _applyPromotion;
        private readonly ITimeframeHandle _timeframeHandle;
        private List<Promotion> _promotions;

        public ApplyPromotionHandler(IPromotionHandle promotionHandle, IConditionHandle conditionHandle,
            IApplyPromotion applyPromotion, ITimeframeHandle timeframeHandle)
        {
            _promotionHandle = promotionHandle;
            _timeframeHandle = timeframeHandle;
            _conditionHandle = conditionHandle;
            _applyPromotion = applyPromotion;

        }
        public void SetPromotions(List<Promotion> promotions)
        {
            _promotions = promotions;
        }
        public List<Promotion> GetPromotions()
        {
            return _promotions;
        }
        public override void Handle(OrderResponseModel order)
        {
            Setorder(order);
            #region Check condition
            //ApplyHandle => PromotionHandle => TimeframeHandle(nếu có) => ConditionHandle

            _promotionHandle.SetNext(_timeframeHandle).SetNext(_conditionHandle);
            _promotionHandle.SetPromotions(_promotions);

            _promotionHandle.Handle(order);

            #endregion
            #region Apply action
            _promotions = _conditionHandle.GetPromotions();
            _applyPromotion.SetPromotions(_promotions);
            if (_promotions.Count == 1)
            {
                _applyPromotion.Apply(order);
                return;
            }
            else
            {
                if (order.CustomerOrderInfo.Vouchers != null && order.CustomerOrderInfo.Vouchers.Count > 0)
                {
                    _applyPromotion.Apply(order);
                }
            }
            #endregion
            /*base.Handle(order);*/
        }

        private void Setorder(OrderResponseModel order)
        {
            order.Discount ??= 0;
            order.DiscountOrderDetail ??= 0;
            order.TotalAmount ??= order.CustomerOrderInfo.Amount;
            order.FinalAmount ??= order.CustomerOrderInfo.Amount;
        }
    }
}
