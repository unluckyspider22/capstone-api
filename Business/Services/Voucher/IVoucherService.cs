using Infrastructure.DTOs;
using Infrastructure.Models;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IVoucherService : IBaseService<Voucher, VoucherDto>
    {
        Task<int> get();
    }

}
