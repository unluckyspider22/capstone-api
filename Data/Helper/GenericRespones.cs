using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Helper
{
    public class GenericRespones<T> where T : class
    {
        public MetaData Metadata { get; set; }
        public List<T> Data { get; set; }
        public GenericRespones(List<T> data, MetaData metadata)
        {
            this.Metadata = metadata;
            this.Data = data;
        }

    }
}
