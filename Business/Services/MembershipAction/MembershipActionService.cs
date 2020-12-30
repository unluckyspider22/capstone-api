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
    public class MembershipActionService : IMembershipActionService
    {
        private readonly PromotionEngineContext _context;

        public MembershipActionService(PromotionEngineContext context)
        {
            _context = context;
        }

        public List<MembershipAction> GetMembershipActions()
        {
            return _context.MembershipAction.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).ToList();
        }


        public MembershipAction FindMembershipAction(Guid id)
        {
            var membershipAction = _context.MembershipAction.Find(id);
            if (membershipAction == null || membershipAction.DelFlg == GlobalVariables.DELETED)
            {
                return null;
            }
            return membershipAction;
        }


        public int AddMembershipAction(MembershipAction membershipActionParam)
        {
            membershipActionParam.MembershipActionId = Guid.NewGuid();
            _context.MembershipAction.Add(membershipActionParam);
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

        public int UpdateMembershipAction(Guid id, MembershipActionParam param)
        {
            var Action = _context.MembershipAction.Find(id);
            if (Action != null)
            {
                Action.PromotionTierId = param.PromotionTierId;
                Action.GroupNo = param.GroupNo;
                Action.ActionType = param.ActionType;
                Action.GiftProductCode = param.GiftProductCode;
                Action.GiftName = param.GiftName;
                Action.GiftVoucherCode = param.GiftVoucherCode;
                Action.BonusPoint = param.BonusPoint;
                Action.UpdDate = DateTime.Now;
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
        public int DeleteMembershipAction(Guid id)
        {
            var Action = _context.MembershipAction.Find(id);
            if (Action != null)
            {
                _context.MembershipAction.Remove(Action);
                return _context.SaveChanges();

            }
            return GlobalVariables.FAIL;
        }


    }
}
