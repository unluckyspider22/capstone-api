using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.DTOs.PromotionStoreMapping;
using Infrastructure.DTOs.PromotionTier;
using Infrastructure.DTOs.Role;
using Infrastructure.DTOs.Store;
using Infrastructure.DTOs.Voucher;
using Infrastructure.DTOs.VoucherChannel;
using Infrastructure.DTOs.VoucherGroup;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.AutoMapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            //Acount

            //Action
            CreateMap<Models.Action, ActionDto>();
            CreateMap<ActionDto, Models.Action>();
            //Brand
            CreateMap<Brand, BrandDto>();
            CreateMap<BrandDto, Brand>();
            //Channel

            //ConditionRule

            //Holiday

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
        }
    }
}
