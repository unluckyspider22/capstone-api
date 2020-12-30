using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Utils;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services
{
    public class ConditionRuleService : IConditionRuleService
    {
        private readonly PromotionEngineContext _context;
        public ConditionRuleService(PromotionEngineContext context)
        {
            _context = context;
        }
        public int CountConditionRule()
        {
            return _context.ConditionRule.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).Count();
        }

        public int CreateConditionRule(ConditionRuleParam conditionRuleParam)
        {
            conditionRuleParam.ConditionRuleId = Guid.NewGuid();

            ConditionRule conditionRule = new ConditionRule
            {
                ConditionRuleId = conditionRuleParam.ConditionRuleId,
                BrandId = conditionRuleParam.BrandId,
                RuleName = conditionRuleParam.RuleName,
                Description = conditionRuleParam.Description,

            };
            _context.ConditionRule.Add(conditionRule);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return GlobalVariables.SUCCESS;
        }

        public int DeleteConditionRule(Guid id)
        {
            var conditionRule = _context.ConditionRule.Find(id);

            if (conditionRule == null)
            {
                return GlobalVariables.NOT_FOUND;
            }

            try
            {
                _context.ConditionRule.Remove(conditionRule);
            }
            catch (Exception)
            {
                throw;
            }


            return GlobalVariables.SUCCESS;
        }

        public List<ConditionRule> GetConditionRules()
        {
            return _context.ConditionRule.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).ToList();
        }

        public ConditionRule GetConditionRules(Guid id)
        {
            return _context.ConditionRule
               .Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED))
               .Where(c => c.ConditionRuleId.Equals(id))
               .First();
        }

        public int HideConditionRule(Guid id)
        {
            var conditionRule = _context.ConditionRule.Find(id);

            if (conditionRule == null)
            {
                return GlobalVariables.NOT_FOUND;
            }

            conditionRule.DelFlg = GlobalVariables.DELETED;
            conditionRule.UpdDate = DateTime.Now;

            try
            {
                _context.Entry(conditionRule).Property("DelFlg").IsModified = true;
                _context.Entry(conditionRule).Property("UpdDate").IsModified = true;
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }


            return GlobalVariables.SUCCESS;
        }

        public int UpdateConditionRule(Guid id, ConditionRuleParam conditionRuleParam)
        {
            var conditionRule = _context.ConditionRule.Find(id);

            if (conditionRule == null)
            {
                return GlobalVariables.NOT_FOUND;
            }

            conditionRule.ConditionRuleId = conditionRuleParam.ConditionRuleId;
            conditionRule.BrandId = conditionRuleParam.BrandId;
            conditionRule.RuleName = conditionRuleParam.RuleName;
            conditionRule.Description = conditionRuleParam.Description;
            conditionRule.UpdDate = DateTime.Now;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return GlobalVariables.SUCCESS;
        }
    }
}
