using ApplicationCore.Models.VoucherGroup;
using ApplicationCore.Utils;
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

        public int CountVoucherGroup()
        {
            return _context.VoucherGroup.ToList().Count;
        }

        public int DeleteVoucherGroup(Guid id)
        {
            var voucherGroup = _context.VoucherGroup.Find(id);

            if (voucherGroup == null)
            {
                return GlobalVariables.NOT_FOUND;
            }
            try
            {
                _context.VoucherGroup.Remove(voucherGroup);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }


            return GlobalVariables.SUCCESS;
        }

        public VoucherGroupParam GetVoucherGroup(Guid id)
        {
            var voucherGroup = _context.VoucherGroup.Find(id);
            VoucherGroupParam result = new VoucherGroupParam();
            result.VoucherGroupId = id;
            result.BrandId = voucherGroup.BrandId;
            result.Quantity = voucherGroup.Quantity;
            result.IsActive = voucherGroup.IsActive;
            result.IsPublic = voucherGroup.IsPublic;
            result.RedempedQuantity = voucherGroup.RedempedQuantity;
            result.UsedQuantity = voucherGroup.UsedQuantity;
            result.Status = voucherGroup.Status;

            if (voucherGroup.DelFlg.Equals(GlobalVariables.DELETED))
            {
                // vẫn trả ra obj, FE check delflag để hiển thị ????
                return null;
            }
            return result;
        }

        public List<VoucherGroup> GetVoucherGroups()
        {
            return _context.VoucherGroup.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).ToList();
        }

        public int PostVoucherGroup(VoucherGroup VoucherGroup)
        {
            VoucherGroup.VoucherGroupId = Guid.NewGuid();
            VoucherGroup.InsDate = DateTime.Now;
            _context.VoucherGroup.Add(VoucherGroup);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (VoucherGroupExists(VoucherGroup.VoucherGroupId))
                {
                    return GlobalVariables.DUPLICATE;
                }
                else
                {
                    throw;
                }
            }

            return GlobalVariables.SUCCESS;
        }

        public int PutVoucherGroup(Guid id, VoucherGroupParam VoucherGroupParam)
        {
            var result = _context.VoucherGroup.Find(id);
            
            if(result == null)
            {
                return GlobalVariables.NOT_FOUND;
            }
            result.VoucherGroupId = VoucherGroupParam.VoucherGroupId;
            result.BrandId = VoucherGroupParam.BrandId;
            result.IsActive = VoucherGroupParam.IsActive;
            result.IsPublic = VoucherGroupParam.IsPublic;
            result.Quantity = VoucherGroupParam.Quantity;
            result.RedempedQuantity = VoucherGroupParam.RedempedQuantity;
            result.UsedQuantity = VoucherGroupParam.UsedQuantity;
            result.PublicDate = VoucherGroupParam.PublicDate;
            result.Status = VoucherGroupParam.Status;
            result.UpdDate = DateTime.Now;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoucherGroupExists(id))
                {
                    return GlobalVariables.NOT_FOUND;
                }
                else
                {
                    throw;
                }
            }

            return GlobalVariables.SUCCESS;
        }

        public int UpdateDelFlag(Guid id, string delflg)
        {
            var store = _context.VoucherGroup.Find(id);
            if (store == null)
            {
                return GlobalVariables.NOT_FOUND;
            }
            store.UpdDate = DateTime.Now;
            store.DelFlg = delflg;

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

       

        private bool VoucherGroupExists(Guid id)
        {
            return _context.VoucherGroup.Any(e => e.VoucherGroupId == id);
        }
    }
}
