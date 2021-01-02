using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Models;
using Infrastructure.Repository;

namespace Infrastructure.UnitOrWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private PromotionEngineContext _context;
        private bool _disposed = false;

        public UnitOfWork(PromotionEngineContext context)
        {
            _context = context;
            initRepository();
        }
        //Account

        //Action
        public IGenericRepository<Models.Action> ActionRepository { get; set; }
        //Brand
        public IGenericRepository<Brand> BrandRepository { get; set; }
        //Channel

        //ConditionRule

        //Holiday

        //Membership

        //MembershipAction

        //MembershipCondition

        //OrderAction

        //ProductAction

        //Promotion

        //PromotionStoreMapping

        //PromotionTier

        //RoleEntity

        //Store

        //Voucher

        //VoucherChannel

        //VoucherGroup


        private void initRepository()
        {
            //Account

            //Action
            ActionRepository = new GenericRepository<Models.Action>(_context);
            //Brand
            BrandRepository = new GenericRepository<Brand>(_context);
            //Channel

            //ConditionRule

            //Holiday

            //Membership

            //MembershipAction

            //MembershipCondition

            //OrderAction

            //ProductAction

            //Promotion

            //PromotionStoreMapping

            //PromotionTier

            //RoleEntity

            //Store

            //Voucher

            //VoucherChannel

            //VoucherGroup

        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
