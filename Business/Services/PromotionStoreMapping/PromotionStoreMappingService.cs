using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Utils;
using ApplicationCore.Models.PromotionStoreMapping;

namespace ApplicationCore.Services
{
    public class PromotionStoreMappingService : IPromotionStoreMappingService
    {
        private readonly PromotionEngineContext _context;
        public PromotionStoreMappingService(PromotionEngineContext context)
        {
            _context = context;
        }

        public int DeletePromotionStoreMapping(Guid id)
        {
            var promotionStoreMapping =  _context.PromotionStoreMapping.Find(id);
            if (promotionStoreMapping == null)
            {
                return GlobalVariables.NOT_FOUND;
            }

            try
            {
                _context.PromotionStoreMapping.Remove(promotionStoreMapping);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return GlobalVariables.SUCCESS;
        }

        public PromotionStoreMappingParam GetPromotionStoreMapping(Guid id)
        {
            var promotionStoreMapping = _context.PromotionStoreMapping.Find(id);
            PromotionStoreMappingParam result = new PromotionStoreMappingParam();
            result.Id = id;
            result.PromotionId = promotionStoreMapping.PromotionId;
            result.StoreId = promotionStoreMapping.StoreId;
            
            if (promotionStoreMapping.DelFlg.Equals(GlobalVariables.DELETED))
            {
                // vẫn trả ra obj, FE check delflag để hiển thị ????
                return null;
            }
            return result;
        }

        public List<PromotionStoreMapping> GetPromotionStoreMappings()
        {
            return _context.PromotionStoreMapping.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).ToList();
        }

        public int PostPromotionStoreMapping(PromotionStoreMapping promotionStoreMapping)
        {
            promotionStoreMapping.Id = Guid.NewGuid();
            promotionStoreMapping.InsDate = DateTime.Now;
            _context.PromotionStoreMapping.Add(promotionStoreMapping);
            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PromotionStoreMappingExists(promotionStoreMapping.Id))
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

        public int PutPromotionStoreMapping(PromotionStoreMapping promotionStoreMapping)
        {
            /*var insDate = _context.PromotionStoreMapping.Find(promotionStoreMapping.Id).InsDate;
            promotionStoreMapping.InsDate = DateTime.Now;*/
            promotionStoreMapping.UpdDate = DateTime.Now;
            _context.Entry(promotionStoreMapping).State = EntityState.Modified;

            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PromotionStoreMappingExists(promotionStoreMapping.Id))
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
            var promotionStoreMapping = _context.PromotionStoreMapping.Find(id);
            if (promotionStoreMapping == null)
            {
                return GlobalVariables.NOT_FOUND;
            }
            promotionStoreMapping.UpdDate = DateTime.Now;
            promotionStoreMapping.DelFlg = delflg;

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

        

        private bool PromotionStoreMappingExists(Guid id)
        {
            return _context.PromotionStoreMapping.Any(e => e.Id == id);
        }
    }
}
