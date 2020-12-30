using ApplicationCore.Models;
using ApplicationCore.Utils;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Services
{
    public class ProductConditionService : IProductConditionService
    {
        private readonly PromotionEngineContext _context;

        public ProductConditionService(PromotionEngineContext context)
        {
            _context = context;
        }
        public List<ProductCondition> GetProductConditions()
        {
            return _context.ProductCondition.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).ToList();
        }

        public ProductCondition FindProductCondition(Guid id)
        {
            var ProductCondition = _context.ProductCondition.Find(id);
            if (ProductCondition == null || ProductCondition.DelFlg == GlobalVariables.DELETED)
            {
                return null;
            }
            return ProductCondition;
        }

        public int AddProductCondition(ProductCondition param)
        {
            param.ProductConditionId = Guid.NewGuid();
            _context.ProductCondition.Add(param);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return GlobalVariables.DUPLICATE;
            }
            return GlobalVariables.SUCCESS;
        }
        public int UpdateProductCondition(Guid id, ProductConditionParam param)
        {
            var condition = _context.ProductCondition.Find(id);
            if (condition != null)
            {
                condition.ConditionRuleId = param.ConditionRuleId;
                condition.GroupNo = param.GroupNo;
                condition.ProductConditionType = param.ProductConditionType;
                condition.ProductType = param.ProductType;
                condition.ProductCode = param.ProductCode;
                condition.ProductQuantity = param.ProductQuantity;
                condition.ProductTag = param.ProductTag;

                try
                {
                    int result = _context.SaveChanges();
                    if (result > 0)
                    {
                        return GlobalVariables.SUCCESS;
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return GlobalVariables.DUPLICATE;
                }
            }

            return GlobalVariables.NOT_FOUND;
        }

        public int DeleteProductCondition(Guid id)
        {
            var condition = _context.ProductCondition.Find(id);
            if (condition != null)
            {
                _context.ProductCondition.Remove(condition);
                return _context.SaveChanges();

            }
            return GlobalVariables.FAIL;
        }
    }
}
