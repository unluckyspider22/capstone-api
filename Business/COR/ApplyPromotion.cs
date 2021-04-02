using ApplicationCore.Request;
using ApplicationCore.Services;
using Infrastructure.Helper;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Chain
{
    public interface IApplyPromotion
    {
        void Apply(OrderResponseModel order);
        void SetPromotions(List<Promotion> promotions);

    }
    public class ApplyPromotion : IApplyPromotion
    {
        private List<Promotion> _promotions;
        private readonly IVoucherService _voucherService;
        public void SetPromotions(List<Promotion> promotions)
        {
            _promotions = promotions;
        }

        public ApplyPromotion(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        public void Apply(OrderResponseModel order)
        {
            order.TotalAmount = order.CustomerOrderInfo.Amount + order.CustomerOrderInfo.ShippingFee;
            foreach (var promotion in _promotions)
            {
                //Lấy những Tier có ID đc thỏa hết các điều kiện
                var promotionTiers =
                    promotion.PromotionTier.Where(el =>
                        order.Effects.Any(a => a.PromotionTierId == el.PromotionTierId)
                ).ToList();

                PromotionTier applyTier = FilterTier(promotionTiers, promotion);
                if (applyTier != null)
                {
                    var action = applyTier.Action;
                    var postAction = applyTier.PostAction;
                    if (action != null)
                    {
                        if (action.ActionType >= 1 && action.ActionType <= 3)
                        {
                            DiscountOrder(order, action, promotion, applyTier);
                        }
                        else
                        {
                            DiscountProduct(order, action);
                        }
                    }
                    if (postAction != null)
                    {
                        if (postAction.PostActionType >= 1 && postAction.PostActionType <= 2)
                        {
                            AddGift(order, postAction, promotion, applyTier);

                        }
                        else
                        {
                            AddPoint(order, postAction, promotion, applyTier);
                        }
                    }
                }
                SetFinalAmountApply(order);
            }
        }
        #region Filter Action & Post Action
        private PromotionTier FilterTier(List<PromotionTier> tiers, Promotion promotion)
        {
            PromotionTier result = null;
            if (tiers.Count > 0 && tiers.Count == 1)
            {
                return tiers.First();
            }
            else
            {
                if (promotion.ActionType != 0)
                {
                    switch (promotion.ActionType)
                    {
                        case (int)AppConstant.EnvVar.ActionType.Amount_Order:
                            result = tiers.FirstOrDefault(w =>
                                 w.Action.ActionType == (int)AppConstant.EnvVar.ActionType.Amount_Order
                                 && w.Action.DiscountAmount > 0
                                 && w.Action.DiscountAmount == tiers.Select(s => s.Action).Max(m => m.DiscountAmount));

                            break;
                        case (int)AppConstant.EnvVar.ActionType.Percentage_Order:
                            result = tiers.FirstOrDefault(w =>
                                 w.Action.ActionType == (int)AppConstant.EnvVar.ActionType.Percentage_Order
                                 && w.Action.DiscountPercentage > 0
                                 && w.Action.DiscountPercentage == tiers.Select(s => s.Action).Max(m => m.DiscountPercentage));

                            break;
                        case (int)AppConstant.EnvVar.ActionType.Shipping:
                            result = tiers.FirstOrDefault(w =>
                                 w.Action.ActionType == (int)AppConstant.EnvVar.ActionType.Shipping
                                 && w.Action.DiscountAmount > 0
                                 && w.Action.DiscountAmount == tiers.Select(s => s.Action).Max(m => m.DiscountAmount));
                            break;
                    }
                }
                else
                {
                    result = tiers.Where(w =>
                    w.TierIndex == tiers.Max(m => m.TierIndex)).SingleOrDefault();
                }
            }

            return result;
        }
        private PostAction FilterPostAction(List<PromotionTier> tiers)
        {
            PostAction result = null;
            if (tiers.Count() > 0 && tiers.Count() == 1)
            {
                return tiers.First().PostAction;
            }
            else
            {
                result = tiers.Where(w =>
                    w.TierIndex == tiers.Max(m => m.TierIndex))
                    .SingleOrDefault().PostAction;
            }
            return result;
        }
        private Infrastructure.Models.Action FilterAction(List<Infrastructure.Models.Action> actions, Promotion promotion)
        {
            Infrastructure.Models.Action result = null;
            if (actions.Count() > 0 && actions.Count() == 1)
            {
                return actions.First();
            }
            else
            {
                switch (promotion.ActionType)
                {
                    case (int)AppConstant.EnvVar.ActionType.Amount_Order:
                        result = actions
                        .Where(w =>
                            w.ActionType == (int)AppConstant.EnvVar.ActionType.Amount_Order
                            && w.DiscountAmount > 0
                            && w.DiscountAmount == actions.Max(m => m.DiscountAmount))
                        .SingleOrDefault();
                        break;
                    case (int)AppConstant.EnvVar.ActionType.Percentage_Order:
                        result = actions.Where(w =>
                                w.ActionType == (int)AppConstant.EnvVar.ActionType.Percentage_Order &&
                                w.DiscountPercentage > 0 &&
                                w.DiscountPercentage == actions.Max(m => m.DiscountPercentage))
                        .SingleOrDefault();
                        break;
                    case (int)AppConstant.EnvVar.ActionType.Shipping:
                        result = actions.Where(w =>
                                w.ActionType == (int)AppConstant.EnvVar.ActionType.Shipping &&
                                w.DiscountAmount > 0 &&
                                w.DiscountAmount == actions.Max(m => m.DiscountAmount))
                        .SingleOrDefault();
                        break;
                }
            }
            return result;
        }
        #endregion
        #region Post action
        public void AddGift(OrderResponseModel order, PostAction postAction, Promotion promotion, PromotionTier promotionTier)
        {
            if (order.Gift == null)
            {
                order.Gift = new List<Gift>();
            }
            string effectType = "";
            switch (postAction.PostActionType)
            {
                case (int)AppConstant.EnvVar.PostActionType.Gift_Product:
                    effectType = AppConstant.EffectMessage.AddGiftProduct;

                    var gifts = postAction.PostActionProductMapping.Select(el => el.Product);
                    foreach (var gift in gifts)
                    {
                        order.Gift.Add(new Gift
                        {
                            ProductCode = gift.Code,
                            ProductName = gift.Name
                        });
                    }
                    break;
                case (int)AppConstant.EnvVar.PostActionType.Gift_Voucher:
                    effectType = AppConstant.EffectMessage.AddGiftVoucher;
                    var voucher = _voucherService.GetFirst(filter: el =>
                                             el.VoucherGroupId == postAction.GiftVoucherGroupId
                                             && !el.IsRedemped
                                && !el.IsUsed,
                                includeProperties: "Promotion").Result;

                    order.Gift.Add(new Gift
                    {
                        ProductCode = voucher.Promotion.PromotionCode + "-" + voucher.VoucherCode,
                        ProductName = voucher.VoucherGroup.VoucherName
                    });
                    break;
                case (int)AppConstant.EnvVar.PostActionType.Gift_Point:
                    effectType = AppConstant.EffectMessage.AddGiftPoint;
                    AddPoint(order, postAction, promotion, promotionTier);
                    break;
            }

            SetEffect(order, promotion, 0, effectType, promotionTier);
        }

        public void AddPoint(OrderResponseModel order, PostAction postAction, Promotion promotion, PromotionTier promotionTier)
        {
            string effectType = AppConstant.EffectMessage.AddGiftPoint;
            order.BonusPoint = postAction.BonusPoint;
            SetEffect(order, promotion, 0, effectType, promotionTier);
        }
        #endregion
        #region Discount Order
        private void DiscountOrder(OrderResponseModel order, Infrastructure.Models.Action action, Promotion promotion, PromotionTier promotionTier)
        {
            decimal discount = 0;
            var final = (decimal)order.CustomerOrderInfo.Amount - (order.CustomerOrderInfo.CartItems.Sum(s => s.Discount)
                + order.CustomerOrderInfo.CartItems.Sum(s => s.DiscountFromOrder));

            var effectType = "";
            switch (action.ActionType)
            {
                case (int)AppConstant.EnvVar.ActionType.Percentage_Order:
                    discount = (decimal)final * (decimal)action.DiscountPercentage / 100;
                    discount = discount > (decimal)action.MaxAmount ? (decimal)action.MaxAmount : discount;
                    effectType = AppConstant.EffectMessage.SetDiscount;
                    discount = discount > (decimal)final ? (decimal)final : discount;
                    SetDiscountFromOrder(order, discount, final, promotion);
                    break;
                case (int)AppConstant.EnvVar.ActionType.Amount_Order:
                    discount = (decimal)action.DiscountAmount;
                    effectType = AppConstant.EffectMessage.SetDiscount;

                    discount = discount > (decimal)final ? (decimal)final : discount;
                    SetDiscountFromOrder(order, discount, final, promotion);
                    break;
                case (int)AppConstant.EnvVar.ActionType.Shipping:
                    discount = (decimal)action.DiscountAmount;
                    effectType = AppConstant.EffectMessage.SetShippingFee;
                    order.CustomerOrderInfo.ShippingFee -= discount;
                    order.CustomerOrderInfo.ShippingFee = order.CustomerOrderInfo.ShippingFee > 0 ? order.CustomerOrderInfo.ShippingFee : 0;
                    break;
            }
            SetEffect(order, promotion, discount, effectType, promotionTier);
        }

        public void SetEffect(OrderResponseModel order, Promotion promotion, decimal discount, string effectType, PromotionTier promotionTier)
        {
            if (order.Effects == null)
            {
                order.Effects = new List<Effect>();
            }
            Effect effect = new Effect
            {
                PromotionId = promotion.PromotionId,
                PromotionTierId = promotionTier.PromotionTierId,
                ConditionRuleName = promotionTier.ConditionRule.RuleName,
                TierIndex = promotionTier.TierIndex,
                EffectType = effectType,
            };
            if (promotionTier.Action != null)
            {
                effect.Prop = new
                {
                    name = promotionTier.Summary,
                    value = discount
                };
                order.Effects.Add(effect);
            }
            if (promotionTier.PostAction != null)
            {
                effect.Prop = new
                {
                    gifts = promotionTier.PostAction.PostActionProductMapping.Select(s =>
                    {
                        string listProduct = "";
                        listProduct += s.Product.Name;
                        return listProduct;
                    })
                };
                order.Effects.Add(effect);
            }
            if (order.Effects.Count() == 0)
            {
                order.Effects = null;
            }

        }
        private void SetDiscountFromOrder(OrderResponseModel order, decimal discount, decimal final, Promotion promotion)
        {
            var discountPercent = discount / final;

            order.CustomerOrderInfo.CartItems = order.CustomerOrderInfo.CartItems.Select(el =>
            {
                var finalAmount = el.TotalAmount - el.Discount;
                el.DiscountFromOrder += Math.Round((finalAmount - el.DiscountFromOrder) * discountPercent, 2);
                el.FinalAmount = finalAmount;
                return el;
            }).ToList();
        }

        #endregion
        #region Discount for item
        private void DiscountProduct(OrderResponseModel order, Infrastructure.Models.Action action)
        {
            var actionProducts = action.ActionProductMapping;
            if (!action.ActionType.Equals(AppConstant.EnvVar.ActionType.Bundle))
            {
                foreach (var product in order.CustomerOrderInfo.CartItems)
                {
                    if (actionProducts.Any(a => a.Product.Code.Equals(product.ProductCode)))
                    {
                        switch (action.ActionType)
                        {
                            case (int)AppConstant.EnvVar.ActionType.Amount_Product:
                                DiscountProductAmount(product, action);
                                break;
                            case (int)AppConstant.EnvVar.ActionType.Percentage_Product:
                                DiscountProductPercentage(product, action);
                                break;
                            case (int)AppConstant.EnvVar.ActionType.Unit:
                                DiscountProductUnit(product, action);
                                break;
                            case (int)AppConstant.EnvVar.ActionType.Fixed:
                                DiscountProductFixedPrice(product, action);
                                break;
                            case (int)AppConstant.EnvVar.ActionType.Ladder:
                                DiscountProductLadderPrice(product, action);
                                break;

                        }
                    }
                    product.FinalAmount = product.TotalAmount - product.Discount;
                }
            }
            else
            {
                var products = order.CustomerOrderInfo.CartItems;
                int countProductMatch = 0;
                foreach (var product in products)
                {
                    if (actionProducts.Any(a => a.Product.Code.Equals(product.ProductCode)))
                    {
                        countProductMatch += (int)(product.Quantity > action.BundleQuantity ? action.BundleQuantity : product.Quantity);
                    }
                }
                if (countProductMatch >= action.BundleQuantity)
                {
                    DiscountProductBundlePrice(products, action);
                }
            }
        }
        private void DiscountProductAmount(Item product, Infrastructure.Models.Action action)
        {
            decimal discount = (decimal)action.DiscountAmount;
            SetDiscountProduct(product, action, discount);
        }
        private void DiscountProductPercentage(Item product, Infrastructure.Models.Action action)
        {
            decimal discount = product.TotalAmount * (decimal)action.DiscountPercentage / 100;
            SetDiscountProduct(product, action, discount);
        }
        private void DiscountProductUnit(Item product, Infrastructure.Models.Action action)
        {
            decimal discount = 0;
            if (product.Quantity >= action.DiscountQuantity)
            {
                discount = (decimal)(action.DiscountQuantity * product.UnitPrice);
            }
            SetDiscountProduct(product, action, discount);
        }
        private void DiscountProductFixedPrice(Item product, Infrastructure.Models.Action action)
        {
            decimal discount = (decimal)(product.TotalAmount - action.FixedPrice * product.Quantity);
            SetDiscountProduct(product, action, discount);
        }
        private void DiscountProductLadderPrice(Item product, Infrastructure.Models.Action action)
        {
            decimal discount = 0;
            if (product.Quantity >= action.OrderLadderProduct)
            {
                discount = (decimal)(product.UnitPrice - action.LadderPrice);
            }
            SetDiscountProduct(product, action, discount);
        }
        private void DiscountProductBundlePrice(List<Item> products, Infrastructure.Models.Action action)
        {
            var actionProducts = action.ActionProductMapping;

            int bundleQuantity = (int)action.BundleQuantity;
            int discountedProduct = 0;
            switch (action.BundleStrategy)
            {
                case (int)AppConstant.BundleStrategy.CHEAPEST:
                    products = products.OrderBy(e => e.UnitPrice).ToList();
                    break;
                case (int)AppConstant.BundleStrategy.MOST_EXPENSIVE:
                    products = products.OrderByDescending(e => e.UnitPrice).ToList();
                    break;
            }
            foreach (var product in products)
            {
                decimal discount = product.Discount;
                if (actionProducts.Any(a => a.Product.Code.Equals(product.ProductCode)))
                {
                    discountedProduct = product.Quantity > bundleQuantity ? bundleQuantity : product.Quantity;
                    discount += (decimal)(product.TotalAmount - discountedProduct * action.BundlePrice);

                    bundleQuantity -= discountedProduct;
                    SetDiscountProduct(product, action, discount);
                }
                if (bundleQuantity <= 0)
                {
                    break;
                }
            }
        }
        private void SetDiscountProduct(Item product, Infrastructure.Models.Action action, decimal discount)
        {
            product.Discount += discount;
            if (action.ActionType.Equals(AppConstant.EnvVar.ActionType.Amount_Product))
            {
                product.Discount = product.Discount < action.MinPriceAfter ? (decimal)action.MinPriceAfter : product.Discount;
            }
            else if (action.ActionType.Equals(AppConstant.EnvVar.ActionType.Percentage_Product))
            {
                product.Discount = (decimal)(product.Discount > action.MaxAmount ? action.MaxAmount : product.Discount);
            }
        }

        #endregion
        private void SetFinalAmountApply(OrderResponseModel order)
        {
            order.DiscountOrderDetail = order.CustomerOrderInfo.CartItems.Sum(s => s.Discount);
            order.Discount = (decimal)order.CustomerOrderInfo.CartItems.Sum(s => s.DiscountFromOrder)
                + (decimal)order.DiscountOrderDetail;
            order.FinalAmount = Math.Ceiling((decimal)(order.TotalAmount - order.Discount));
        }
    }
}
