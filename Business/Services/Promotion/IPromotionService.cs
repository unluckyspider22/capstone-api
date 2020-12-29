using ApplicationCore.Models;
using Infrastructure.Models;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Services
{
    public interface IPromotionService
    {
        public List<Promotion> GetPromotions();

        public Promotion FindPromotion(Guid id);

        public int AddPromotion(Promotion param);
        public int UpdatePromotion(Guid id, Promotion param);

        public int DeletePromotion(Guid id);
    }
}
