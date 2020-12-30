using ApplicationCore.Models.Store;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IStoreService

    {
        public List<Store> GetStores();

        public Store GetStore(Guid id);

        public int PostStore(Store store);

        public int PutStore(Guid id, StoreParam storeParam);

        public int DeleteStore(Guid id);

        public int CountStore();

        public int UpdateDelFlag(Guid id,string delflg);
    }
}
