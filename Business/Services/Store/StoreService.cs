using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Utils;
using ApplicationCore.Models.Store;

namespace ApplicationCore.Services
{
    public class StoreService : IStoreService
    {
        private readonly PromotionEngineContext _context;
        public StoreService(PromotionEngineContext context)
        {
            _context = context;
        }

        public int CountStore()
        {
            return _context.Store.ToList().Count;
        }

        public int DeleteStore(Guid id)
        {
            var store = _context.Store.Find(id);
            if (store == null)
            {
                return GlobalVariables.NOT_FOUND;
            }

            _context.Store.Remove(store);
            _context.SaveChangesAsync();
            return GlobalVariables.SUCCESS;
        }

        public StoreParam GetStore(Guid id)
        {
            var store = _context.Store.Find(id);
            StoreParam result = new StoreParam();
            result.StoreId = id;
            result.StoreCode = store.StoreCode;
            result.StoreName = store.StoreName;
            result.BrandId = store.BrandId;
            if (store.DelFlg != GlobalVariables.DELETED)
            {
                // vẫn trả ra obj, FE check delflag để hiển thị ????
                return null;
            }
            return result != null ? result : null;
        }

        public List<Store> GetStores()
        {        
            return _context.Store.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).ToList();
        }

        public int PostStore(Store store)
        {
            store.StoreId = Guid.NewGuid();
            _context.Store.Add(store);
            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StoreExists(store.StoreId))
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

        public int PutStore(Store store)
        {
            store.UpdDate = DateTime.Now;
            _context.Entry(store).State = EntityState.Modified;

            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(store.StoreId))
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
        private bool StoreExists(Guid id)
        {
            return _context.Store.Any(e => e.StoreId == id);
        }
    }
}

