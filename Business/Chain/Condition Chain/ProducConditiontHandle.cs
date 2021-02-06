using ApplicationCore.Models;
using ApplicationCore.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Chain
{
    public interface IProductConditionHandle : IHandler<ConditionModel>
    {

    }
    public class ProducConditiontHandle : Handler<ConditionModel>, IProductConditionHandle
    {
    }
}
