using Infrastructure.Models;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Models;

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
        

        Task<int> SaveAsync();
    }
}
