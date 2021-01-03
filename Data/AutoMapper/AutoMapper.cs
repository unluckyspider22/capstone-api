﻿using AutoMapper;
using Infrastructure.DTOs;
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
    }
}
