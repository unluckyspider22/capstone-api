using Infrastructure.Models;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.UnitOrWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<Brand> BrandRepository { get; }
        Task<int> SaveAsync();
    }
}
