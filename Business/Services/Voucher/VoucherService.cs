using ApplicationCore.Models.Voucher;
using ApplicationCore.Utils;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly PromotionEngineContext _context;
        public VoucherService(PromotionEngineContext context)
        {
            _context = context;
        }

        public int CountVoucher()
        {
            return _context.Voucher.ToList().Count;
        }

        public int DeleteVoucher(Guid id)
        {
            var Voucher = _context.Voucher.Find(id);

            if (Voucher == null)
            {
                return GlobalVariables.NOT_FOUND;
            }
            try
            {
                _context.Voucher.Remove(Voucher);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }


            return GlobalVariables.SUCCESS;
        }

        public Voucher GetVoucher(Guid id)
        {
            var Voucher = _context.Voucher.Find(id);
            /* VoucherParam result = new VoucherParam();
             result.VoucherId = id;
             result.VoucherCode = Voucher.VoucherCode;
             result.VoucherName = Voucher.VoucherName;
             result.BrandId = Voucher.BrandId;*/
            if (Voucher.DelFlg.Equals(GlobalVariables.DELETED))
            {
                // vẫn trả ra obj, FE check delflag để hiển thị ????
                return null;
            }
            return Voucher;
        }

        public List<Voucher> GetVouchers()
        {
            return _context.Voucher.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).ToList();
        }

        public int PostVoucher(Voucher Voucher)
        {
            Voucher.VoucherId = Guid.NewGuid();
            Voucher.InsDate = DateTime.Now;
            _context.Voucher.Add(Voucher);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (VoucherExists(Voucher.VoucherId))
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

        public int PutVoucher(Guid id, VoucherParam VoucherParam)
        {
            var result = _context.Voucher.Find(id);

            if (result == null)
            {
                return GlobalVariables.NOT_FOUND;
            }
            result.VoucherId = VoucherParam.VoucherId;
            result.VoucherCode = VoucherParam.VoucherCode;
            result.VoucherChannelId = VoucherParam.VoucherChannelId;
            result.UpdDate = DateTime.Now;
            result.MembershipId = VoucherParam.MembershipId;
            result.IsUsed = VoucherParam.IsUsed;
            result.IsRedemped = VoucherParam.IsRedemped;
            result.UsedDate = VoucherParam.UsedDate;
            result.RedempedDate = VoucherParam.RedempedDate;
            result.IsActive = VoucherParam.IsActive;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoucherExists(id))
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
            var Voucher = _context.Voucher.Find(id);
            if (Voucher == null)
            {
                return GlobalVariables.NOT_FOUND;
            }
            Voucher.UpdDate = DateTime.Now;
            Voucher.DelFlg = delflg;

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

        private bool VoucherExists(Guid id)
        {
            return _context.Voucher.Any(e => e.VoucherId == id);
        }
    }
}
