
using Infrastructure.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IVoucherGroupService : IBaseService<VoucherGroup, VoucherGroupDto>
    {
        public List<VoucherDto> GenerateBulkCodeVoucher(VoucherGroupDto dto);

        public List<VoucherDto> GenerateStandaloneVoucher(VoucherGroupDto dto);
        public Task<IEnumerable<VoucherParamResponse>> GetVoucherForGame(int PageIndex = 0, int PageSize = 0, string BrandCode = null, string StoreCode = null);
        // Ẩn voucher group
        public Task<bool> HideVoucherGroup(Guid id);
        // Xóa hoàn toàn voucher group
        public Task<bool> DeleteVoucherGroup(Guid id);
        // Reject voucher group ra khỏi promotion
        public Task<bool> RejectVoucherGroup(Guid id);
        // Assign voucher group cho promotion
        public Task<bool> AssignVoucherGroup(Guid promotionId, Guid voucherGroupId);
        public Task<List<VoucherDto>> CreateVoucherBulk(List<VoucherDto> vouchers);
    }
}
