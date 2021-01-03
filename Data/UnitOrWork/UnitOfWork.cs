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
        public IGenericRepository<Account> AccountRepositoryImp { get; set; }
        //Action
        public IGenericRepository<Models.Action> ActionRepository { get; set; }
        //Brand
        public IGenericRepository<Brand> BrandRepository { get; set; }
        //Channel
        public IGenericRepository<Channel> ChannelRepository { get; set; }
        //ConditionRule
        public IGenericRepository<ConditionRule> ConditionRuleRepository { get; set; }
        //Holiday
        public IGenericRepository<Holiday> HolidayRepository { get; set; }

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
            AccountRepositoryImp = new AccountRepository(_context);
            //Action
            ActionRepository = new GenericRepository<Models.Action>(_context);
            //Brand
            BrandRepository = new GenericRepository<Brand>(_context);
            //Channel
            ChannelRepository = new GenericRepository<Channel>(_context);
            //ConditionRule
            ConditionRuleRepository = new GenericRepository<ConditionRule>(_context);
            //Holiday
            HolidayRepository = new GenericRepository<Holiday>(_context);
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
