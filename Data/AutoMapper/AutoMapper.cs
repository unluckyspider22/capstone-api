using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;

namespace Infrastructure.AutoMapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            //Acount
            CreateMap<Account, AccountDto>();
            CreateMap<AccountDto, Account>();
            //Action
            CreateMap<Models.Action, ActionDto>();
            CreateMap<ActionDto, Models.Action>();
            //Brand
            CreateMap<Brand, BrandDto>();
            CreateMap<BrandDto, Brand>();
            //Channel
            CreateMap<Channel, ChannelDto>();
            CreateMap<ChannelDto, Channel>();
            //ConditionRule
            CreateMap<ConditionRule, ConditionRuleDto>();
            CreateMap<ConditionRuleDto, ConditionRule>();
            //Holiday
            CreateMap<Holiday, HolidayDto>();
            CreateMap<HolidayDto, Holiday>();
            //Membership
            CreateMap<Membership, MembershipDto>();
            CreateMap<MembershipDto, Membership>();
            //MembershipAction
            CreateMap<MembershipAction, MembershipActionDto>();
            CreateMap<MembershipActionDto, MembershipAction>();
            //MembershipCondition
            CreateMap<MembershipCondition, MembershipConditionDto>();
            CreateMap<MembershipConditionDto, MembershipCondition>();
            //OrderCondition
            CreateMap<OrderCondition, OrderConditionDto>();
            CreateMap<OrderConditionDto, OrderCondition>();
            //ProductCondition
            CreateMap<ProductCondition, ProductConditionDto>();
            CreateMap<ProductConditionDto, ProductCondition>();
            //Promotion
            CreateMap<Promotion, PromotionDto>();
            CreateMap<PromotionDto, Promotion>();
            //PromotionStoreMapping
            CreateMap<PromotionStoreMapping, PromotionStoreMappingDto>();
            CreateMap<PromotionStoreMappingDto, PromotionStoreMapping>();
            //PromotionTier
            CreateMap<PromotionTier, PromotionTierDto>();
            CreateMap<PromotionTierDto, PromotionTier>();
            //RoleEntity
            CreateMap<RoleEntity, RoleDto>();
            CreateMap<RoleDto, RoleEntity>();
            //Store
            CreateMap<Store, StoreDto>();
            CreateMap<StoreDto, Store>();
            //Voucher
            CreateMap<Voucher, VoucherDto>();
            CreateMap<VoucherDto, Voucher>();
            //VoucherChannel
            CreateMap<VoucherChannel, VoucherChannelDto>();
            CreateMap<VoucherChannelDto, VoucherChannel>();
            //VoucherGroup
            CreateMap<VoucherGroup, VoucherGroupDto>();
            CreateMap<VoucherGroupDto, VoucherGroup>();
            //VoucherGroup
            CreateMap<ConditionGroup, ConditionGroupDto>();
            CreateMap<ConditionGroupDto, ConditionGroup>();
            //ConditionRequestParam
            CreateMap<ConditionRule, ConditionRequestParam>();
            CreateMap<ConditionRequestParam, ConditionRule>();
            //Action request param
            CreateMap<Models.Action, ActionRequestParam>();
            CreateMap<ActionRequestParam, Models.Action>();
            //Membership request param
            CreateMap<MembershipAction, MembershipActionRequestParam>();
            CreateMap<MembershipActionRequestParam, MembershipAction>();
        }
    }
}
