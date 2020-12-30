using ApplicationCore.Models.PromotionTier;
using ApplicationCore.Utils;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class PromotionTierService
    {

        private readonly PromotionEngineContext _context;
        public PromotionTierService(PromotionEngineContext context)
        {
            _context = context;
        }

        public int CountPromotionTier()
        {
            return _context.PromotionTier.ToList().Count;
        }

        public int DeletePromotionTier(Guid id)
        {
            var PromotionTier = _context.PromotionTier.Find(id);

            if (PromotionTier == null)
            {
                return GlobalVariables.NOT_FOUND;
            }
            try
            {
                _context.PromotionTier.Remove(PromotionTier);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }


            return GlobalVariables.SUCCESS;
        }

        public PromotionTier GetPromotionTier(Guid id)
        {
            var PromotionTier = _context.PromotionTier.Find(id);
            /* PromotionTierParam result = new PromotionTierParam();
             result.PromotionTierId = id;
             result.PromotionTierCode = PromotionTier.PromotionTierCode;
             result.PromotionTierName = PromotionTier.PromotionTierName;
             result.BrandId = PromotionTier.BrandId;*/
            if (PromotionTier.DelFlg.Equals(GlobalVariables.DELETED))
            {
                // vẫn trả ra obj, FE check delflag để hiển thị ????
                return null;
            }
            return PromotionTier;
        }

        public List<PromotionTier> GetPromotionTiers()
        {
            return _context.PromotionTier.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).ToList();
        }

        public int PostPromotionTier(PromotionTier PromotionTier)
        {
            PromotionTier.PromotionTierId = Guid.NewGuid();
            PromotionTier.InsDate = DateTime.Now;
            _context.PromotionTier.Add(PromotionTier);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PromotionTierExists(PromotionTier.PromotionTierId))
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

        public int PutPromotionTier(Guid id, PromotionTierParam PromotionTierParam)
        {
            var result = _context.PromotionTier.Find(id);

            if (result == null)
            {
                return GlobalVariables.NOT_FOUND;
            }
            result.PromotionTierId = PromotionTierParam.PromotionTierId;
            result.PromotionId = PromotionTierParam.PromotionId;
            result.ActionId = PromotionTierParam.ActionId;
            result.UpdDate = DateTime.Now;
            result.ConditionRuleId = PromotionTierParam.ConditionRuleId;
            result.MembershipActionId = PromotionTierParam.MembershipActionId;
            result.VoucherGroupId = PromotionTierParam.VoucherGroupId;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PromotionTierExists(id))
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
            var PromotionTier = _context.PromotionTier.Find(id);
            if (PromotionTier == null)
            {
                return GlobalVariables.NOT_FOUND;
            }
            PromotionTier.UpdDate = DateTime.Now;
            PromotionTier.DelFlg = delflg;

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

        private bool PromotionTierExists(Guid id)
        {
            return _context.PromotionTier.Any(e => e.PromotionTierId == id);
        }

    }
}
