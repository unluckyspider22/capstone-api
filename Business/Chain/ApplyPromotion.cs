using ApplicationCore.Request;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public void SetPromotions(List<Promotion> promotions)
        {
            _promotions = promotions;
        }
        public void Apply(OrderResponseModel order)
        {
            foreach (var promotion in _promotions)
            {
                //Lấy những Tier có ID đc thỏa hết các điều kiện
                var promotionTiers =
                    promotion.PromotionTier.Where(el =>
                        order.Effects.Any(a => a.PromotionTierId == el.PromotionTierId)
                ).ToList();

                var action = FilterAction(promotionTiers.Select(el => el.Action).ToList(), promotion);
                if (action != null)
                {

                    switch (action.ActionType)
                    {
                        case AppConstant.EnvVar.ActionType.Order:
                            DiscountOrder(order, action, promotion);
                            break;
                        case AppConstant.EnvVar.ActionType.Product:
                            DiscountProduct(order, action);
                            break;
                        case AppConstant.EnvVar.ActionType.Gift:
                            DiscountProduct(order, action);
                            break;
                        case AppConstant.EnvVar.ActionType.BonusPoint:
                            DiscountProduct(order, action);
                            break;

                    }
                }
                SetFinalAmountApply(order);
            }
        }
        private Infrastructure.Models.Action FilterAction(List<Infrastructure.Models.Action> actions, Promotion promotion)
        {
            Infrastructure.Models.Action result = null;
            if (actions.Count() > 0 && actions.Count() == 1)
            {
                return result;
            }
            else
            {
                switch (promotion.DiscountType)
                {
                    case AppConstant.EnvVar.DiscountType.Amount:
                        result = actions
                        .Where(w =>
                            w.DiscountType == AppConstant.EnvVar.DiscountType.Amount
                            && w.DiscountAmount > 0
                            && w.DiscountAmount == actions.Max(m => m.DiscountAmount))
                        .SingleOrDefault();
                        break;
                    case AppConstant.EnvVar.DiscountType.Percentage:
                        result = actions.Where(w =>
                                w.DiscountType == AppConstant.EnvVar.DiscountType.Percentage &&
                                w.DiscountPercentage > 0 &&
                                w.DiscountPercentage == actions.Max(m => m.DiscountPercentage))
                        .SingleOrDefault();
                        break;
                    case AppConstant.EnvVar.DiscountType.Shipping:
                        result = actions.Where(w =>
                                w.DiscountType == AppConstant.EnvVar.DiscountType.Shipping &&
                                w.DiscountPercentage > 0 &&
                                w.DiscountPercentage == actions.Max(m => m.DiscountPercentage))
                        .SingleOrDefault();
                        break;
                }
            }
            return result;
        }
        #region Discount Order
        private void DiscountOrder(OrderResponseModel order, Infrastructure.Models.Action action, Promotion promotion)
        {
            decimal discount = 0;
            var final = (decimal)order.CustomerOrderInfo.Amount - (order.CustomerOrderInfo.CartItems.Sum(s => s.Discount)
                + order.CustomerOrderInfo.CartItems.Sum(s => s.DiscountFromOrder));

            if (action.DiscountType.Equals(AppConstant.EnvVar.DiscountType.Percentage))
            {
                discount = (decimal)final * (decimal)action.DiscountPercentage / 100;
                discount = discount > (decimal)action.MaxAmount ? (decimal)action.MaxAmount : discount;

            }
            else if (action.DiscountType.Equals(AppConstant.EnvVar.DiscountType.Amount))
            {
                discount = (decimal)action.DiscountAmount;
            }
            discount = discount > (decimal)final ? (decimal)final : discount;
            /*throw new ErrorObj(code: 400, message: "discount: " + discount + ", final: " + final);*/
            SetDiscountFromOrder(order, discount, final, promotion);

            if (order.Effects == null)
            {
                order.Effects = new List<Effect>();
            }
            order.Effects.Add(new
                Effect
            {
                PromotionId = promotion.PromotionId,
                PromotionTierId = action.PromotionTier.PromotionTierId,
                ConditionRuleName = action.PromotionTier.ConditionRule.RuleName,
                TierIndex = (int)action.PromotionTier.TierIndex,
                EffectType = AppConstant.EffectMessage.SetDiscount,
                Prop = new
                {
                    name = action.PromotionTier.Summary,
                    value = discount
                }
            });

        }
        private void SetDiscountFromOrder(OrderResponseModel order, decimal discount, decimal final, Promotion promotion)
        {
            var discountPercent = discount / final;

            order.CustomerOrderInfo.CartItems = order.CustomerOrderInfo.CartItems.Select(el =>
            {
                var finalAmount = el.TotalAmount - (el.DiscountFromOrder + el.Discount);
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
            if (!action.DiscountType.Equals(AppConstant.EnvVar.DiscountType.Bundle))
            {
                foreach (var product in order.CustomerOrderInfo.CartItems)
                {
                    if (actionProducts.Any(a => a.Product.Code.Equals(product.ProductCode)))
                    {
                        switch (action.DiscountType)
                        {
                            case AppConstant.EnvVar.DiscountType.Amount:
                                DiscountProductAmount(product, action);
                                break;
                            case AppConstant.EnvVar.DiscountType.Percentage:
                                DiscountProductPercentage(product, action);
                                break;
                            case AppConstant.EnvVar.DiscountType.Unit:
                                DiscountProductUnit(product, action);
                                break;
                            case AppConstant.EnvVar.DiscountType.Fixed:
                                DiscountProductFixedPrice(product, action);
                                break;
                            case AppConstant.EnvVar.DiscountType.Ladder:
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
                case AppConstant.BundleStrategy.CHEAPEST:
                    products = products.OrderBy(e => e.UnitPrice).ToList();
                    break;
                case AppConstant.BundleStrategy.MOST_EXPENSIVE:
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
            if (action.DiscountType.Equals(AppConstant.EnvVar.DiscountType.Amount))
            {
                product.Discount = product.Discount < action.MinPriceAfter ? (decimal)action.MinPriceAfter : product.Discount;
            }
            else if (action.DiscountType.Equals(AppConstant.EnvVar.DiscountType.Percentage))
            {
                product.Discount = (decimal)(product.Discount > action.MaxAmount ? action.MaxAmount : product.Discount);
            }
        }

        #endregion
        private void SetFinalAmountApply(OrderResponseModel order)
        {
            order.TotalAmount = order.CustomerOrderInfo.Amount;
            order.DiscountOrderDetail = order.CustomerOrderInfo.CartItems.Sum(s => s.Discount);
            order.Discount = (decimal)order.CustomerOrderInfo.CartItems.Sum(s => s.DiscountFromOrder)
                + (decimal)order.DiscountOrderDetail;
            order.FinalAmount = Math.Ceiling((decimal)(order.TotalAmount - order.Discount));
        }
    }
}
