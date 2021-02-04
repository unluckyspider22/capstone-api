using ApplicationCore.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Chain
{
    public interface IMembershipHandle : IHandler<OrderResponseModel>
    {

    }
    public class MembershipHandle : Handler<OrderResponseModel>, IMembershipHandle
    {
        
    }
}
