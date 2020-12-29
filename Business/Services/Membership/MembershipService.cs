
using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services
{
    public class MembershipService : IMembershipService
    {
        private static int DUPLICATE = 0;
        private static int OK = 1;
        private static int FAIL = 2;
        private readonly PromotionEngineContext _context;

        public MembershipService(PromotionEngineContext context)
        {
            _context = context;
        }

        public List<Membership> FindMembership()
        {
            return _context.Membership.Where(c => c.DelFlg.Equals("0")).ToList();
        }

        public Membership FindMembership(Guid id)
        {
            var membership = _context.Membership.Find(id);
            if (membership.DelFlg != "0")
            {
                return null;
            }
            return membership != null ? membership : null;
        }

        public Membership UpdateMembership(Guid id, Membership membership)
        {

            if (id != membership.MembershipId)
            {
                return null;
            }
            else
                _context.Entry(membership).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return null;
        }

        public int AddMembership(Membership membership)
        {
            _context.Membership.Add(membership);
            try
            {
                if (_context.SaveChanges() > 1)
                {
                    return OK;
                }
            }
            catch (DbUpdateException)
            {
                return DUPLICATE;
            }
            return FAIL;
        }

        public int DeleteMembership(Guid id)
        {
            var membership = _context.Membership.Find(id);
            if (membership != null)
            {
                _context.Membership.Remove(membership);
                return _context.SaveChanges();
                
            }
            return FAIL;
        }





    }
}
