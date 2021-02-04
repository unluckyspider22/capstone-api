using ApplicationCore.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Chain
{
    public interface IOrderHandle : IHandler<OrderResponseModel>
    {

    }
    public class OrderHandle : Handler<OrderResponseModel>, IOrderHandle
    {
    }
}
