using AutoMapper;
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
            //Brand
            CreateMap<Brand, BrandDto>()
                .ForAllMembers(opt => opt.Condition(
                    (source, destination, sourceMember, destMember) => (sourceMember != null)
                    )
                );
            CreateMap<BrandDto, Brand>();
        }
    }
}
