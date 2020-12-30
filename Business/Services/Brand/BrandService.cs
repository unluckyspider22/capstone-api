using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Utils;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Services
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
            return _context.Brand.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).Count();
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
            catch (DbUpdateException)
            {
                throw;
            }

            return GlobalVariables.SUCCESS;
        }

        public int DeleteBrand(Guid id)
        {
            var brand = _context.Brand.Find(id);

            if (brand == null)
            {
                return GlobalVariables.NOT_FOUND;
            }

            
            try
            {
                _context.Brand.Remove(brand);
            }
            catch (Exception)
            {
                throw;
            }


            return GlobalVariables.SUCCESS;
        }

        public List<Brand> GetBrands()
        {
            return _context.Brand.Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED)).ToList();
        }

        public Brand GetBrands(Guid id)
        {
            return _context.Brand
                .Where(c => !c.DelFlg.Equals(GlobalVariables.DELETED))
                .Where(c => c.BrandId.Equals(id))
                .First();
        }

        public int HideBrand(Guid id)
        {
            var brand = _context.Brand.Find(id);

            if (brand == null)
            {
                return GlobalVariables.NOT_FOUND;
            }

            brand.DelFlg = GlobalVariables.DELETED;
            brand.UpdDate = DateTime.Now;
            try
            {
                _context.Entry(brand).Property("DelFlg").IsModified = true;
                _context.Entry(brand).Property("UpdDate").IsModified = true;
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }


            return GlobalVariables.SUCCESS;
        }

        public int UpdateBrand(Guid id, BrandParam brandParam)
        {
            var brand = _context.Brand.Find(id);

            if (brand == null)
            {
                return GlobalVariables.NOT_FOUND;
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
            brand.UpdDate = DateTime.Now;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return GlobalVariables.SUCCESS;

        }
    }
}
