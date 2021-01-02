using Infrastructure.Models;
using Infrastructure.Repository;
using System.Threading.Tasks;

namespace Infrastructure.UnitOrWork
{
    public interface IUnitOfWork
    {
        //Account

        //Action
        IGenericRepository<Models.Action> ActionRepository { get; }
        //Brand
        IGenericRepository<Brand> BrandRepository { get; }
        //Channel

        //ConditionRule

        //Holiday

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
        IGenericRepository<VoucherChannel> VoucherChannelRepository { get; }
        //VoucherGroup
        IGenericRepository<VoucherGroup> VoucherGroupRepository { get; }

        Task<int> SaveAsync();
    }
}
