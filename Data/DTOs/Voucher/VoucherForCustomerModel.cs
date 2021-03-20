using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTOs.Voucher
{
    public class VoucherForCustomerModel
    {
        public string CusFullName { get; set; }
        public string CusEmail { get; set; }
        public string CusPhoneNo { get; set; }
        public string CusGender { get; set; }
        public string ChannelCode { get; set; }

    }
}
