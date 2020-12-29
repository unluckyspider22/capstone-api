using ApplicationCore.Models;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IOrderConditionService
    {
        public List<OrderCondition> GetOrderConditions();

        public OrderCondition FindOrderCondition(Guid id);

        public int AddOrderCondition(OrderCondition param);
        public int UpdateOrderCondition(Guid id, OrderConditionParam param);

        public int DeleteOrderCondition(Guid id);
    }
}
