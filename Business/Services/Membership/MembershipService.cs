
using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Models;
using ApplicationCore.Utils;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly PromotionEngineContext _context;

        public MembershipService(PromotionEngineContext context)
        {
            _context = context;
        }

        public List<Membership> GetMembership()
        {
            return _context.Membership.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).ToList();
        }

        public Membership FindMembership(Guid id)
        {
            var membership = _context.Membership.Find(id);
            if (membership == null || membership.DelFlg == GlobalVariables.DELETED)
            {
                return null;
            }
            return membership;
        }

        public int UpdateMembership(Guid id, MembershipParam param)
        {
            var member = _context.Membership.Find(id);
            if (member != null)
            {
                member.Fullname = param.Fullname;
                member.Email = param.Email;
                member.MembershipCode = param.MembershipCode;
                member.UpdDate = DateTime.Now;
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

        public int AddMembership(Membership membership)
        {
            membership.MembershipId = Guid.NewGuid();
            _context.Membership.Add(membership);
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

        public int DeleteMembership(Guid id)
        {
            var membership = _context.Membership.Find(id);
            if (membership != null)
            {
                _context.Membership.Remove(membership);
                return _context.SaveChanges();

            }
            return GlobalVariables.FAIL;
        }
    }
}
