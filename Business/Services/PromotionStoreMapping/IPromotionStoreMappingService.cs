using ApplicationCore.Models.PromotionStoreMapping;
using Infrastructure.DTOs.PromotionStoreMapping;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IPromotionStoreMappingService : IBaseService<PromotionStoreMapping,PromotionStoreMappingDto>
    {
      
    }
}
