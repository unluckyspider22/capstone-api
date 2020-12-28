using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services.Stores
{
    public interface IStoreService

    {
        public List<Store> GetStores();

        public Store GetStore(Guid id);

        public int PostStore(Store promotionStoreMapping);

        public int PutStore(Store promotionStoreMapping);

        public int DeleteStore(Guid id);
    }
}
