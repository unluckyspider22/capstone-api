
using Infrastructure.DTOs;
using Infrastructure.Helper;
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
        //public Task<IEnumerable<VoucherGroup>> GetVoucherGroupForGame(int PageIndex = 0, int PageSize = 0, string BrandCode = null, string StoreCode = null);
        // Ẩn voucher group
        public Task<bool> HideVoucherGroup(Guid id);
        // Xóa hoàn toàn voucher group
        public Task<bool> DeleteVoucherGroup(Guid id);
        // Reject voucher group ra khỏi promotion
        public Task<bool> RejectVoucherGroup(Guid voucherGroupId, Guid promotionId);
        // Assign voucher group cho promotion
        public Task<bool> AssignVoucherGroup(Guid promotionId, Guid voucherGroupId);
        public Task<List<VoucherDto>> CreateVoucherBulk(List<VoucherDto> vouchers);
        public Task<List<Voucher>> MapVoucher(List<VoucherDto> vouchers);
        public Task<String> GetPromotionCode(Guid promotionId);

        public Task UpdateRedempedQuantity(VoucherGroup voucherGroup, int RedempedQuantity);

        public Task UpdateVoucherGroupForApplied(VoucherGroup voucherGroup);

        public Task<bool> AddMoreVoucher(Guid voucherGroupId, int quantity);

        public Task<GenericRespones<AvailableVoucherDto>> GetAvailable(int PageSize, int PageIndex, Guid BrandId);
    }
}
