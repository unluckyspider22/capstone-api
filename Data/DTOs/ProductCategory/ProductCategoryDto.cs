using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class ProductCategoryDto : BaseDto
    {
        public Guid ProductCateId { get; set; }
        public Guid BrandId { get; set; }
        [StringLength(30)]
        public string CateId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
    }
}
