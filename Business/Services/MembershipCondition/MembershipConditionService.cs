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
    public class MembershipConditionService : IMembershipConditionService
    {
        private readonly PromotionEngineContext _context;

        public MembershipConditionService(PromotionEngineContext context)
        {
            _context = context;
        }

        public List<MembershipCondition> GetMembershipConditions()
        {
            return _context.MembershipCondition.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).ToList();
        }


        public MembershipCondition FindMembershipCondition(Guid id)
        {
            var membershipCondition = _context.MembershipCondition.Find(id);
            if (membershipCondition ==null || membershipCondition.DelFlg == GlobalVariables.DELETED)
            {
                return null;
            }
            return membershipCondition;
        }


        public int AddMembershipCondition(MembershipCondition membershipConditionParam)
        {
            membershipConditionParam.MembershipConditionId = Guid.NewGuid();
            _context.MembershipCondition.Add(membershipConditionParam);
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

        public int UpdateMembershipCondition(Guid id, MembershipConditionParam param)
        {
            var condition = _context.MembershipCondition.Find(id);
            if (condition != null)
            {
                condition.ConditionRuleId = param.ConditionRuleId;
                condition.GroupNo = param.GroupNo;
                condition.ForNewMember = param.ForNewMember;
                condition.MembershipLevel = param.MembershipLevel;
                condition.UpdDate = DateTime.Now;
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
        public int DeleteMembershipCondition(Guid id)
        {
            var condition = _context.MembershipCondition.Find(id);
            if (condition != null)
            {
                _context.MembershipCondition.Remove(condition);
                return _context.SaveChanges();

            }
            return GlobalVariables.FAIL;
        }


    }
}
