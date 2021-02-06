using ApplicationCore.Request;
using Infrastructure.DTOs;
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
        public override void Handle(OrderResponseModel request)
        {
            /*throw new ErrorObj(code: 400, message: "MembershipHandle");*/

            base.Handle(request);
        }

    }
}
