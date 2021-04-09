using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    public partial class Transaction
    {
        public Guid Id { get; set; }
        public Guid BrandId { get; set; }
        public string TransactionJson { get; set; }
        public DateTime InsDate { get; set; }
        public DateTime UpdDate { get; set; }
        public Guid? VoucherId { get; set; }
        public Guid? PromotionId { get; set; }

        public virtual Brand Brand { get; set; }
    }
}
