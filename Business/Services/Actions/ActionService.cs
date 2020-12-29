
using ApplicationCore.Models;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ApplicationCore.Services.Actions
{
    public class ActionService : IActionService
    {
        private readonly PromotionEngineContext _context;
        public ActionService(PromotionEngineContext context)
        {
            _context = context;
        }
        public int CountAction()
        {
            int count = _context.Action.Where(c => c.DelFlg.Equals("0")).Count();
            return count;
        }

        public int CreateAction(ActionParam actionParam)
        {
            Infrastructure.Models.Action action = new Infrastructure.Models.Action
            {
                ActionId = actionParam.ActionId,
                PromotionTierId = actionParam.PromotionTierId,
                GroupNo = actionParam.GroupNo,
                ActionType = actionParam.ActionType,
                IsLimitAmount = actionParam.IsLimitAmount,
                ProductCode = actionParam.ProductCode,
                DiscountQuantity = actionParam.DiscountQuantity,
                DiscountAmount = actionParam.DiscountAmount,
                DiscountPercentage = actionParam.DiscountPercentage,
                FixedPrice = actionParam.FixedPrice,
                MaxAmount = actionParam.MaxAmount,
                ForCurrentProduct = actionParam.ForCurrentProduct,
                ApplyLadderPrice = actionParam.ApplyLadderPrice
            };

            _context.Action.Add(action);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return 0;
            }

            return 1;
        }

        public int DeleteAction(System.Guid id)
        {
            var action = _context.Action.Find(id);
            if (action == null)
            {
                return 0;
            }
            action.DelFlg = "1";
            _context.Entry(action).Property("DelFlg").IsModified = true;
            _context.SaveChanges();

            return 1;
        }

        public List<Infrastructure.Models.Action> GetActions()
        {
            var action = _context.Action.Where(c => c.DelFlg.Equals("0")).ToList();

            return action;
        }

        public Infrastructure.Models.Action GetActions(Guid id)
        {
            var action = _context.Action.Where(c => c.DelFlg.Equals("0")).Where(c => c.ActionId.Equals(id)).FirstOrDefault();

            return action;
        }

        public int UpdateAction(System.Guid id, ActionParam actionParam)
        {
            var action = _context.Action.Find(id);
            if (action == null)
            {
                return 0;
            }
            action.GroupNo = actionParam.GroupNo;
            action.ActionType = actionParam.ActionType;
            action.IsLimitAmount = actionParam.IsLimitAmount;
            action.ProductCode = actionParam.ProductCode;
            action.DiscountQuantity = actionParam.DiscountQuantity;
            action.DiscountAmount = actionParam.DiscountAmount;
            action.DiscountPercentage = actionParam.DiscountPercentage;
            action.FixedPrice = actionParam.FixedPrice;
            action.MaxAmount = actionParam.MaxAmount;
            action.ForCurrentProduct = actionParam.ForCurrentProduct;
            action.ApplyLadderPrice = actionParam.ApplyLadderPrice;
            action.UpdDate = DateTime.Now;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return 0;
            }

            return 1;
        }
    }
}
