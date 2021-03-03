using Infrastructure.Models;
using Infrastructure.Repository;
using System.Threading.Tasks;

namespace Infrastructure.UnitOrWork
{
    public interface IUnitOfWork
    {
        //Account
        IGenericRepository<Account> AccountRepository { get; }
        //Action
        IGenericRepository<Models.Action> ActionRepository { get; }
        //Brand
        IGenericRepository<Brand> BrandRepository { get; }
        //Channel
        IGenericRepository<Channel> ChannelRepository { get; }
        //ConditionRule
        IGenericRepository<ConditionRule> ConditionRuleRepository { get; }
        //Holiday
        IGenericRepository<Holiday> HolidayRepository { get; }
        //Membership
        IGenericRepository<Membership> MembershipRepository { get; }
        //MembershipAction
        IGenericRepository<MembershipAction> MembershipActionRepository { get; }
        //MembershipCondition
        IGenericRepository<MembershipCondition> MembershipConditionRepository { get; }
        //OrderCondition
        IGenericRepository<OrderCondition> OrderConditionRepository { get; }
        //ProductCondition
        IGenericRepository<ProductCondition> ProductConditionRepository { get; }
        //Promotion
        IGenericRepository<Promotion> PromotionRepository { get; }
        //PromotionStoreMapping
        IGenericRepository<PromotionStoreMapping> PromotionStoreMappingRepository { get; }
        //PromotionTier
        IGenericRepository<PromotionTier> PromotionTierRepository { get; }
        //RoleEntity
        IGenericRepository<RoleEntity> RoleEntityRepository { get; }
        //Store
        IGenericRepository<Store> StoreRepository { get; }
        //Voucher
        IGenericRepository<Voucher> VoucherRepository { get; }
        //VoucherChannel
        IGenericRepository<PromotionChannelMapping> VoucherChannelRepository { get; }
        //VoucherGroup
        IGenericRepository<VoucherGroup> VoucherGroupRepository { get; }
        //Condition group
        IGenericRepository<ConditionGroup> ConditionGroupRepository { get; }

        Task<int> SaveAsync();
    }
}
