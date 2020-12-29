using ApplicationCore.Models;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services.Brands
{
    public interface IBrandService
    {
        public List<Brand> GetBrands();
        public Brand GetBrands(System.Guid id);
        public int CreateBrand(BrandParam brandParam);
        public int UpdateBrand(System.Guid id, BrandParam brandParam);
        public int DeleteBrand(System.Guid id);
        public int CountBrand();
    }
}
