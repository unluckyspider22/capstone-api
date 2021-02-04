using ApplicationCore.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Chain
{
    public interface ITimeframeHandle : IHandler<OrderResponseModel>
    {

    }
    public class TimeframeHandle : Handler<OrderResponseModel>, ITimeframeHandle
    {
    }
}
