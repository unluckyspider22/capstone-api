using ApplicationCore.Models;
using ApplicationCore.Request;
using ApplicationCore.Utils;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Chain
{
    public interface IProductConditionHandle : IHandler<OrderResponseModel>
    {
        void SetConditionModel(ConditionModel condition);

    }
    public class ProducConditiontHandle : Handler<OrderResponseModel>, IProductConditionHandle
    {
        private ConditionModel _condition;

        public override void Handle(OrderResponseModel order)
        {
            if (_condition is ProductConditionModel)
            {
                var products = order.CustomerOrderInfo.CartItems;
                HandleIncludeExclude((ProductConditionModel)_condition, products);
                HandleQuantity((ProductConditionModel)_condition, products);
            }
            else
            {
                base.Handle(order);
            }
        }
        public void SetConditionModel(ConditionModel condition)
        {
            _condition = condition;
        }

        private void HandleIncludeExclude(ProductConditionModel productCondition, List<Item> products)
        {
            productCondition.IsMatch = false;
            foreach (var product in products)
            {
                bool isMatch = product.ProductCode.Equals(productCondition.ProductCode);
                if (productCondition.ProductType.Equals(AppConstant.EnvVar.ProductType.SINGLE_PRODUCT))
                {
                    if (productCondition.ProductConditionType.Equals(AppConstant.EnvVar.EXCLUDE))
                    {
                        productCondition.IsMatch = !isMatch;
                    }
                    else productCondition.IsMatch = isMatch;
                }
               /* else
                {
                    if (product.ParentCode.Equals(productCondition.ParentCode)
                        && productCondition.ProductConditionType.Equals(AppConstant.EnvVar.EXCLUDE))
                    {
                        productCondition.IsMatch = !isMatch;
                    }
                    else productCondition.IsMatch = isMatch;
                }*/
                if (productCondition.IsMatch)
                {
                    break;
                }
            }
        }
        private void HandleQuantity(ProductConditionModel condition, List<Item> products)
        {
            if (condition.ProductQuantity > 0)
            {
                foreach (var product in products)
                {
                    condition.IsMatch = Common.Compare<int>(
                        condition.QuantityOperator,
                        product.Quantity,
                        (int)condition.ProductQuantity);
                    if (condition.IsMatch)
                    {
                        break;
                    }
                }
            }

        }
    }
}
