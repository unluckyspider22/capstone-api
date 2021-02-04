using ApplicationCore.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Chain
{
    public interface IProductHandle : IHandler<OrderResponseModel>
    {

    }
    public class ProductHandle : Handler<OrderResponseModel>, IProductHandle
    {
    }
}
