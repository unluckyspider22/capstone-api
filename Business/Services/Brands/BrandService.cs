using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Utils;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services.Brands
{
    public class BrandService : IBrandService
    {
        private readonly PromotionEngineContext _context;
        public BrandService(PromotionEngineContext context)
        {
            _context = context;
        }
        public int CountBrand()
        {
            int count = _context.Brand.Where(c => c.DelFlg.Equals("0")).Count();
            return count;
        }

        public int CreateBrand(BrandParam brandParam)
        {
            Brand brand = new Brand
            {
                BrandId = brandParam.BrandId,
                Username = brandParam.Username,
                BrandCode = brandParam.BrandCode,
                PhoneNumber = brandParam.PhoneNumber,
                ImgUrl = brandParam.ImgUrl,
                BrandName = brandParam.BrandName,
                CompanyName = brandParam.CompanyName,
                Description = brandParam.Description,
                Address = brandParam.Address
            };


            _context.Brand.Add(brand);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {

                Console.WriteLine("Error: " + e.ToString());
                return 0;
            }

            return 1;
        }

        public int DeleteBrand(Guid id)
        {
            var brand = _context.Brand.Find(id);
            if (brand == null)
            {
                return 0;
            }
            brand.DelFlg = "1";
            _context.Entry(brand).Property("DelFlg").IsModified = true;
            _context.SaveChanges();

            return 1;
        }

        public List<Brand> GetBrands()
        {
            return _context.Brand.Where(c => c.DelFlg.Equals("0")).ToList();
        }

        public Brand GetBrands(Guid id)
        {
            return _context.Brand.Where(c => c.DelFlg.Equals("0")).Where(c => c.BrandId.Equals(id)).First();
        }

        public int UpdateBrand(Guid id, BrandParam brandParam)
        {
            var brand = _context.Brand.Find(id);
            if (brand == null)
            {
                return 0;
            }
            brand.BrandId = brandParam.BrandId;
            brand.Username = brandParam.Username;
            brand.BrandCode = brandParam.BrandCode;
            brand.PhoneNumber = brandParam.PhoneNumber;
            brand.ImgUrl = brandParam.ImgUrl;
            brand.BrandName = brandParam.BrandName;
            brand.CompanyName = brandParam.CompanyName;
            brand.Description = brandParam.Description;
            brand.Address = brandParam.Address;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return 0;
            }

            return 1;

        }
    }
}
