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
        public IGenericRepository<Membership> MembershipRepository { get; set; }
        //MembershipAction
        public IGenericRepository<MembershipAction> MembershipActionRepository { get; set; }
        //MembershipCondition
        public IGenericRepository<MembershipCondition> MembershipConditionRepository { get; set; }
        //OrderCondition
        public IGenericRepository<OrderCondition> OrderConditionRepository { get; set; }
        //ProductCondition
        public IGenericRepository<ProductCondition> ProductConditionRepository { get; set; }
        //Promotion
        public IGenericRepository<Promotion> PromotionRepository { get; set; }

        //PromotionStoreMapping
        public IGenericRepository<PromotionStoreMapping> PromotionStoreMappingRepository { get; set; }

        //PromotionTier
        public IGenericRepository<PromotionTier> PromotionTierRepository { get; set; }


        //RoleEntity
        public IGenericRepository<RoleEntity> RoleEntityRepository { get; set; }


        //Store
        public IGenericRepository<Store> StoreRepository { get; set; }


        //Voucher
        public IGenericRepository<Voucher> VoucherRepository { get; set; }


        //VoucherChannel
        public IGenericRepository<VoucherChannel> VoucherChannelRepository { get; set; }


        //VoucherGroup
        public IGenericRepository<VoucherGroup> VoucherGroupRepository { get; set; }



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
            MembershipRepository = new GenericRepository<Membership>(_context);
            //MembershipAction
            MembershipActionRepository = new GenericRepository<MembershipAction>(_context);
            //MembershipCondition
            MembershipConditionRepository = new GenericRepository<MembershipCondition>(_context);
            //OrderCondition
            OrderConditionRepository = new GenericRepository<OrderCondition>(_context);
            //ProductCondition
            ProductConditionRepository = new GenericRepository<ProductCondition>(_context);
            //Promotion
            PromotionRepository = new GenericRepository<Promotion>(_context);
            //PromotionStoreMapping
            PromotionStoreMappingRepository = new GenericRepository<PromotionStoreMapping>(_context);
            //PromotionTier
            PromotionTierRepository = new GenericRepository<PromotionTier>(_context);
            //RoleEntity
            RoleEntityRepository = new GenericRepository<RoleEntity>(_context);
            //Store
            StoreRepository = new GenericRepository<Store>(_context);
            //Voucher
            VoucherRepository = new GenericRepository<Voucher>(_context);
            //VoucherChannel
            VoucherChannelRepository = new GenericRepository<VoucherChannel>(_context);
            //VoucherGroup
            VoucherGroupRepository = new GenericRepository<VoucherGroup>(_context);
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
