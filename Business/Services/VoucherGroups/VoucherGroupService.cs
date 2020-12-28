using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services.VoucherGroups
{
    public class VoucherGroupService  : IVoucherGroupService
    {
        private readonly PromotionEngineContext _context;
        public VoucherGroupService(PromotionEngineContext context)
        {
            _context = context;
        }

        public int DeleteVoucherGroup(Guid id)
        {
            var VoucherGroup = _context.VoucherGroup.Find(id);
            if (VoucherGroup == null)
            {
                return 0;
            }

            _context.VoucherGroup.Remove(VoucherGroup);
            _context.SaveChangesAsync();
            return 1;
        }

        public VoucherGroup GetVoucherGroup(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<VoucherGroup> GetVoucherGroups()
        {
            return _context.VoucherGroup.ToList();
        }

        public int PostVoucherGroup(VoucherGroup VoucherGroup)
        {
            _context.VoucherGroup.Add(VoucherGroup);
            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VoucherGroupExists(VoucherGroup.VoucherGroupId))
                {
                    return 0;
                }
                else
                {
                    throw;
                }
            }

            return 1;
        }

        public int PutVoucherGroup(VoucherGroup VoucherGroup)
        {

            _context.Entry(VoucherGroup).State = EntityState.Modified;

            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoucherGroupExists(VoucherGroup.VoucherGroupId))
                {
                    return 0;
                }
                else
                {
                    throw;
                }
            }

            return 1;
        }
        private bool VoucherGroupExists(Guid id)
        {
            return _context.VoucherGroup.Any(e => e.VoucherGroupId == id);
        }
    }
}
