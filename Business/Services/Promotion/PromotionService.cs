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
    public class PromotionService : IPromotionService
    {
        private readonly PromotionEngineContext _context;

        public PromotionService(PromotionEngineContext context)
        {
            _context = context;
        }
        public List<Promotion> GetPromotions()
        {
            return _context.Promotion.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).ToList();
        }

        public Promotion FindPromotion(Guid id)
        {
            var Promotion = _context.Promotion.Find(id);
            if (Promotion == null || Promotion.DelFlg == GlobalVariables.DELETED)
            {
                return null;
            }
            return Promotion;
        }

        public int AddPromotion(Promotion param)
        {
            param.PromotionId = Guid.NewGuid();
            _context.Promotion.Add(param);
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
        public int UpdatePromotion(Guid id, Promotion param)
        {

            _context.Entry(param).State = EntityState.Modified;
            try
            {
                int result =_context.SaveChanges();
                if(result > 0)
                {
                    return GlobalVariables.SUCCESS;
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return GlobalVariables.DUPLICATE;

            }
            return GlobalVariables.NOT_FOUND;
        }

        public int DeletePromotion(Guid id)
        {
            var condition = _context.Promotion.Find(id);
            if (condition != null)
            {
                _context.Promotion.Remove(condition);
                return _context.SaveChanges();

            }
            return GlobalVariables.FAIL;
        }
    }
}
