using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Utils;
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

            _context.PromotionStoreMapping.Remove(promotionStoreMapping);
             _context.SaveChangesAsync();
            return GlobalVariables.SUCCESS;
        }

        public PromotionStoreMapping GetPromotionStoreMapping(Guid id)
        {
            return _context.PromotionStoreMapping.Find(id);
        }

        public List<PromotionStoreMapping> GetPromotionStoreMappings()
        {
            return _context.PromotionStoreMapping.ToList();
        }

        public int PostPromotionStoreMapping(PromotionStoreMapping promotionStoreMapping)
        {
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
        private bool PromotionStoreMappingExists(Guid id)
        {
            return _context.PromotionStoreMapping.Any(e => e.Id == id);
        }
    }
}
