using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Chain
{
    public interface IOrderConditionHandle : IHandler<ConditionResponseModel>
    {

    }
    public class OrderConditionHandle : Handler<ConditionResponseModel>, IOrderConditionHandle
    {
        public override void Handle(ConditionResponseModel request)
        {

            base.Handle(request);
        }
    }
}
