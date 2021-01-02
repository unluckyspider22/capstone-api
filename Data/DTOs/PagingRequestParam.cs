using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class PagingRequestParam
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
