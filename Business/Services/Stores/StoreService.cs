using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class StoreService : IStoreService
    {
        private readonly PromotionEngineContext _context;
        public StoreService(PromotionEngineContext context)
        {
            _context = context;
        }

        public int DeleteStore(Guid id)
        {
            var Store = _context.Store.Find(id);
            if (Store == null)
            {
                return 0;
            }

            _context.Store.Remove(Store);
            _context.SaveChangesAsync();
            return 1;
        }

        public Store GetStore(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Store> GetStores()
        {
            return _context.Store.ToList();
        }

        public int PostStore(Store Store)
        {
            _context.Store.Add(Store);
            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StoreExists(Store.StoreId))
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

        public int PutStore(Store Store)
        {

            _context.Entry(Store).State = EntityState.Modified;

            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(Store.StoreId))
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
        private bool StoreExists(Guid id)
        {
            return _context.Store.Any(e => e.StoreId == id);
        }
    }
}

