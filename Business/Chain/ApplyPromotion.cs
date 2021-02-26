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

    }
    public class ApplyPromotion : IApplyPromotion
    {
        public void Apply(OrderResponseModel order)
        {
            var promotions = order.Promotions;
            foreach (var promotion in promotions)
            {
                //Lấy những Tier có ID đc thỏa hết các điều kiện
                var promotionTiers = promotion.PromotionTier.Where(el => el.PromotionTierId.Equals(order.PromotionTierIds.FirstOrDefault(w => w.Equals(el.PromotionTierId)))).ToList();

                var actions = FilterAction(promotionTiers.Select(el => el.Action).ToList());
                if (actions.Count() > 0)
                {
                    foreach (var action in actions)
                    {
                        switch (action.ActionType)
                        {
                            case AppConstant.EnvVar.ActionType.Order:
                                DiscountOrder(order, action, promotion);
                                break;
                            case AppConstant.EnvVar.ActionType.Product:
                                DiscountProduct(order, action);
                                break;
                        }
                    }
                }
                SetFinalAmountApply(order);
            }
        }
        private List<Infrastructure.Models.Action> FilterAction(List<Infrastructure.Models.Action> actions)
        {
            var result = new List<Infrastructure.Models.Action>();
            if (actions.Count() > 0 && actions.Count() == 1)
            {
                return actions;
            }
            else
            {
                if (actions.Where(el => el.DiscountType.Equals(AppConstant.EnvVar.DiscountType.Amount)).Count() > 1
                    || actions.Where(el => el.DiscountType.Equals(AppConstant.EnvVar.DiscountType.Percentage)).Count() > 1
                    || actions.Where(el => el.DiscountType.Equals(AppConstant.EnvVar.DiscountType.Shipping)).Count() > 1)
                {
                    var actionAmount = actions
                        .Where(w =>
                        w.DiscountAmount > 0
                        && w.DiscountAmount == actions.Max(m => m.DiscountAmount))
                        .SingleOrDefault();
                    if (actionAmount != null)
                    {
                        result.Add(actionAmount);
                    }
                    var actionPercent = actions.Where(w =>
                    w.DiscountPercentage > 0 && w.DiscountPercentage == actions.Max(m => m.DiscountPercentage)).SingleOrDefault();

                    if (actionPercent != null)
                    {
                        result.Add(actionPercent);
                    }
                }
                else return actions;
            }
            return result;
        }
        #region Discount Order
        private void DiscountOrder(OrderResponseModel order, Infrastructure.Models.Action action, Promotion promotion)
        {
            decimal discount = 0;
            var final = (decimal)order.OrderDetail.Amount - (order.OrderDetail.OrderDetailResponses.Sum(s => s.Discount)
                + order.OrderDetail.OrderDetailResponses.Sum(s => s.DiscountFromOrder));

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

            if (order.OrderAction == null)
            {
                order.OrderAction = new List<OrderActionModel>();
            }
            var promotionCodes = string.IsNullOrEmpty(promotion.PromotionCode) ? promotion.PromotionCode : "," + promotion.PromotionCode;

            var voucherCode = promotion.VoucherGroup.Voucher
                .Where(el =>
                el.VoucherCode.Equals(order.Vouchers
                .Select(s => s.VoucherCode)
                .FirstOrDefault(f => el.VoucherCode.Equals(f))))
                .First().VoucherCode;

            order.OrderAction.Add(new
                OrderActionModel
            {
                ActionId = action.ActionId,
                DiscountAmount = discount,
                PromotionCode = promotionCodes,
                VoucherCode = voucherCode
            });

        }
        private void SetDiscountFromOrder(OrderResponseModel order, decimal discount, decimal final, Promotion promotion)
        {
            var discountPercent = discount / final;

            order.OrderDetail.OrderDetailResponses = order.OrderDetail.OrderDetailResponses.Select(el =>
            {
                var finalAmount = el.TotalAmount - (el.DiscountFromOrder + el.Discount);
                el.DiscountFromOrder += (finalAmount - el.DiscountFromOrder) * discountPercent;
                el.FinalAmount = finalAmount;
                el.PromotionCode = promotion.PromotionCode;
                return el;
            }).ToList();
        }

        #endregion

        #region Discount for item
        private void DiscountProduct(OrderResponseModel order, Infrastructure.Models.Action action)
        {
            if (!action.DiscountType.Equals(AppConstant.EnvVar.DiscountType.Bundle))
            {
                foreach (var product in order.OrderDetail.OrderDetailResponses)
                {
                    if (action.ProductCode.Contains(product.ProductCode))
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
                var products = order.OrderDetail.OrderDetailResponses;
                int countProductMatch = 0;
                foreach (var product in products)
                {
                    if (action.ProductCode.Contains(product.ProductCode))
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
        private void DiscountProductAmount(OrderDetailResponseModel product, Infrastructure.Models.Action action)
        {
            decimal discount = (decimal)action.DiscountAmount;
            SetDiscountProduct(product, action, discount);
        }
        private void DiscountProductPercentage(OrderDetailResponseModel product, Infrastructure.Models.Action action)
        {
            decimal discount = product.TotalAmount * (decimal)action.DiscountPercentage / 100;
            SetDiscountProduct(product, action, discount);
        }
        private void DiscountProductUnit(OrderDetailResponseModel product, Infrastructure.Models.Action action)
        {
            decimal discount = 0;
            if (product.Quantity >= action.DiscountQuantity)
            {
                discount = (decimal)(action.DiscountQuantity * product.UnitPrice);
            }
            SetDiscountProduct(product, action, discount);
        }
        private void DiscountProductFixedPrice(OrderDetailResponseModel product, Infrastructure.Models.Action action)
        {
            decimal discount = (decimal)(product.TotalAmount - action.FixedPrice * product.Quantity);
            SetDiscountProduct(product, action, discount);
        }
        private void DiscountProductLadderPrice(OrderDetailResponseModel product, Infrastructure.Models.Action action)
        {
            decimal discount = 0;
            if (product.Quantity >= action.OrderLadderProduct)
            {
                discount = (decimal)(product.UnitPrice - action.LadderPrice);
            }
            SetDiscountProduct(product, action, discount);
        }
        private void DiscountProductBundlePrice(List<OrderDetailResponseModel> products, Infrastructure.Models.Action action)
        {
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
                if (action.ProductCode.Contains(product.ProductCode))
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
        private void SetDiscountProduct(OrderDetailResponseModel product, Infrastructure.Models.Action action, decimal discount)
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
            order.TotalAmount = order.OrderDetail.Amount;
            order.DiscountOrderDetail = order.OrderDetail.OrderDetailResponses.Sum(s => s.Discount);
            order.Discount = (decimal)order.OrderDetail.OrderDetailResponses.Sum(s => s.DiscountFromOrder)
                + (decimal)order.DiscountOrderDetail;
            order.FinalAmount = Math.Ceiling((decimal)(order.TotalAmount - order.Discount));
        }
    }
}
