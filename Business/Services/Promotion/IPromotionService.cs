﻿using ApplicationCore.Models;
using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IPromotionService : IBaseService<Promotion,PromotionDto>
    {
    }
}
