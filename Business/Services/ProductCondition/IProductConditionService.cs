using ApplicationCore.Models;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IProductConditionService
    {
        public List<ProductCondition> GetProductConditions();

        public ProductCondition FindProductCondition(Guid id);

        public int AddProductCondition(ProductCondition param);
        public int UpdateProductCondition(Guid id, ProductConditionParam param);

        public int DeleteProductCondition(Guid id);
    }
}
