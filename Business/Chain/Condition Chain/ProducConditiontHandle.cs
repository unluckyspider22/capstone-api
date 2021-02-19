﻿using ApplicationCore.Models;
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

                var products = order.OrderDetail.OrderDetailResponses;
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

        private void HandleIncludeExclude(ProductConditionModel condition, List<OrderDetailResponseModel> products)
        {
            _condition.IsMatch = false;
            foreach (var product in products)
            {
                switch (condition.ProductConditionType)
                {
                    case AppConstant.EnvVar.INCLUDE:
                        IsIncludeProduct(condition, product, true);
                        break;
                    case AppConstant.EnvVar.EXCLUDE:
                        IsIncludeProduct(condition, product, false);
                        break;
                }
            }
        }
        private void IsIncludeProduct(ProductConditionModel condition, OrderDetailResponseModel product, bool isInclude)
        {
            if (condition.ProductType.Equals(AppConstant.EnvVar.ProductType.SINGLE_PRODUCT))
            {
                if (product.ProductCode.Equals(condition.ProductCode) && isInclude)
                {
                    _condition.IsMatch = true;
                }
            }
            else
            {
                if (product.ProductCode.Equals(condition.ProductCode)
                    && product.ParentCode.Equals(condition.ParentCode) && isInclude)
                {
                    _condition.IsMatch = true;
                }
            }
        }
        private void HandleQuantity(ProductConditionModel condition, List<OrderDetailResponseModel> products)
        {
            _condition.IsMatch = false;
            foreach (var product in products)
            {
                _condition.IsMatch = Common.Compare<int>(
                    condition.QuantityOperator, 
                    product.Quantity, 
                    (int)condition.ProductQuantity);
                if (_condition.IsMatch)
                {
                    break;
                }
            }
        }
    }
}
