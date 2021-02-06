using ApplicationCore.Models;
using ApplicationCore.Request;
using Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Chain
{
    public interface IMembershipConditionHandle : IHandler<ConditionModel>
    {

    }
    public class MembershipConditionHandle : Handler<ConditionModel>, IMembershipConditionHandle
    {
        public override void Handle(ConditionModel request)
        {
            /*throw new ErrorObj(code: 400, message: "MembershipHandle");*/

            base.Handle(request);
        }

    }
}
