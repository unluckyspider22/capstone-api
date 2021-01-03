using Infrastructure.Models;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.UnitOrWork
{
    public interface IUnitOfWork
    {
        //Account
        IGenericRepository<Account> AccountRepositoryImp { get; }
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
