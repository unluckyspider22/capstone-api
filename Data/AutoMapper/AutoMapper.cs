using ApplicationCore.Models;
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
            //Membership
            CreateMap<Membership, MembershipDto>();
            CreateMap<MembershipDto, Membership>();
            //MembershipAction
            CreateMap<Gift, GiftDto>();
            CreateMap<GiftDto, Gift>();
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
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
            //Store
            CreateMap<Store, StoreDto>();
            CreateMap<StoreDto, Store>();
            //Voucher
            CreateMap<Voucher, VoucherDto>();
            CreateMap<VoucherDto, Voucher>();
            //VoucherChannel
            CreateMap<PromotionChannelMapping, VoucherChannelDto>();
            CreateMap<VoucherChannelDto, PromotionChannelMapping>();
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
            CreateMap<Gift, GiftRequestParam>();
            CreateMap<GiftRequestParam, Gift>();
            //OrderConditionModel
            CreateMap<OrderCondition, OrderConditionModel>();
            CreateMap<OrderConditionModel, OrderCondition>();
            //ProductConditionModel
            CreateMap<ProductCondition, ProductConditionModel>();
            CreateMap<ProductConditionModel, ProductCondition>();

            //Condition rule update param
            CreateMap<ConditionRule, ConditionRuleUpdateParam>();
            CreateMap<ConditionRuleUpdateParam, ConditionRule>();
            //Condition rule update param
            CreateMap<Action, ActionUpdateParam>();
            CreateMap<ActionUpdateParam, Action>();
            // MembershipActionUpdateParam
            CreateMap<Gift, GiftUpdateParam>();
            CreateMap<GiftUpdateParam, Gift>();

            // PromotionInfomation
            CreateMap<PromotionInfomation, Promotion>();
            CreateMap<Promotion, PromotionInfomation>();

            // Store of promotion
            CreateMap<Store, StoreOfPromotion>();
            CreateMap<StoreOfPromotion, Store>();

            // Channel of promotion
            CreateMap<Channel, ChannelOfPromotion>();
            CreateMap<ChannelOfPromotion, Channel>();

            CreateMap<ProductCategory, ProductCategoryDto>();
            CreateMap<ProductCategoryDto, ProductCategory>();

            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();

            CreateMap<Device, DeviceDto>();
            CreateMap<DeviceDto, Device>();

            CreateMap<MemberLevel, MemberLevelDto>();
            CreateMap<MemberLevelDto, MemberLevel>();

            CreateMap<Action, ActionTierDto>();
            CreateMap<ActionTierDto, Action>();

            CreateMap<Gift, GiftTierDto>();
            CreateMap<GiftTierDto, Gift>();

            CreateMap<ProductCondition, ProductConditionTierDto>();
            CreateMap<ProductConditionTierDto, ProductCondition>();

            CreateMap<MemberLevelMapping, MemberLevelMappingDto>();
            CreateMap<MemberLevelMappingDto, MemberLevelMapping>();
            CreateMap<GameCampaign, GameCampaignDto>();
            CreateMap<GameCampaignDto, GameCampaign>();
            CreateMap<GameItemDto, GameItems>();
            CreateMap<GameItems, GameItemDto>();
            CreateMap<GameMasterDto, GameMaster>();
            CreateMap<GameMaster, GameMasterDto>();
        }
    }
}
