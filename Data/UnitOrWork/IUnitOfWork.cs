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

        //PromotionTier

        //RoleEntity

        //Store

        //Voucher

        //VoucherChannel

        //VoucherGroup


        Task<int> SaveAsync();
    }
}
