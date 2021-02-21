using Infrastructure.Helper;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Worker
{
    public interface IVoucherNotify
    {
        public Task ProcessingVoucher(VoucherNotiObj item);
        public Task ProcessedVoucher(VoucherNotiObj item);
        public Task ErrorProcess(VoucherNotiObj item);
    }
    public class VoucherNotify : IVoucherNotify
    {
        private readonly HubConnection connection;
        private const string URL = AppConstant.URL + "voucher/notify";

        public VoucherNotify()
        {
            connection = new HubConnectionBuilder().WithUrl(URL).Build();
            connection.StartAsync();
        }

        public async Task ProcessedVoucher(VoucherNotiObj item)
        {
            await connection.InvokeAsync("DoneNoti", item);
        }

        public async Task ProcessingVoucher(VoucherNotiObj item)
        {
            await connection.InvokeAsync("ProcessingNoti", item);
        }

        public async Task ErrorProcess(VoucherNotiObj item)
        {
            await connection.InvokeAsync("ErrorNoti", item);
        }
    }

    public class VoucherNotiObj
    {
        public Guid Id { get; }
        public Guid? PromotionId { get; set; }
        public Guid VoucherGroupId { get; set; }
        public string Type { get; set; }
        public bool IsDone { get; set; }
        public string Message { get; set; }

        public VoucherNotiObj(Guid? promotionId, Guid voucherGroupId, bool isDone, string message, string type)
        {
            Id = Guid.NewGuid();
            Type = type;
            PromotionId = promotionId;
            VoucherGroupId = voucherGroupId;
            IsDone = isDone;
            Message = message;
        }

        public VoucherNotiObj()
        {
            Id = Guid.NewGuid();
        }
    }
}
