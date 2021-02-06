using ApplicationCore.Models;
using ApplicationCore.Request;
using Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Chain
{
    public interface IOrderConditionHandle : IHandler<ConditionModel>
    {

    }
    public class OrderConditionHandle : Handler<ConditionModel>, IOrderConditionHandle
    {
        public override void Handle(ConditionModel request)
        {
            base.Handle(request);
        }
    }
}
