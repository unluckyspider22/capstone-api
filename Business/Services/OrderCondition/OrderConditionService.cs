using ApplicationCore.Models;
using ApplicationCore.Utils;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class OrderConditionService : IOrderConditionService
    {
        private readonly PromotionEngineContext _context;

        public OrderConditionService(PromotionEngineContext context)
        {
            _context = context;
        }
        public List<OrderCondition> GetOrderConditions()
        {
            return _context.OrderCondition.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).ToList();
        }

        public OrderCondition FindOrderCondition(Guid id)
        {
            var orderCondition = _context.OrderCondition.Find(id);
            if (orderCondition == null || orderCondition.DelFlg == GlobalVariables.DELETED)
            {
                return null;
            }
            return orderCondition;
        }

        public int AddOrderCondition(OrderCondition param)
        {
            param.OrderConditionId = Guid.NewGuid();
            _context.OrderCondition.Add(param);
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
        public int UpdateOrderCondition(Guid id, OrderConditionParam param)
        {
            var condition = _context.OrderCondition.Find(id);
            if (condition != null)
            {
                condition.ConditionRuleId = param.ConditionRuleId;
                condition.GroupNo = param.GroupNo;
                condition.MinQuantity = param.MinQuantity;
                condition.MaxQuantity = param.MaxQuantity;
                condition.MinAmount = param.MaxAmount;
                condition.IsContainProduct = param.IsContainProduct;
                condition.ProductType = param.ProductType;
                condition.ProductCode = param.ProductCode;

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

        public int DeleteOrderCondition(Guid id)
        {
            var condition = _context.OrderCondition.Find(id);
            if (condition != null)
            {
                _context.OrderCondition.Remove(condition);
                return _context.SaveChanges();

            }
            return GlobalVariables.FAIL;
        }
    }
}
