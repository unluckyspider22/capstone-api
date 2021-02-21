using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository.Voucher;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Worker
{
    public interface IVoucherWorker
    {
        public void InsertVouchers(List<Voucher> vouchers, Guid voucherGroupId, Guid? promotionId);
        public void DeleteVouchers(Guid voucherGroupId, Guid? promotionId);
    }
    public class VoucherWorker : IVoucherWorker
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger _logger;
        private readonly CancellationToken _cancellationToken;
        private readonly IVoucherRepository voucherRepository = new VoucherRepositoryImp();
        private readonly IVoucherNotify notify = new VoucherNotify();

        public VoucherWorker(IBackgroundTaskQueue taskQueue,
            ILogger<VoucherWorker> logger,
            IHostApplicationLifetime applicationLifetime)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _cancellationToken = applicationLifetime.ApplicationStopping;
        }

        public void DeleteVouchers(Guid voucherGroupId, Guid? promotionId)
        {
            var item = new VoucherNotiObj()
            {
                PromotionId = promotionId,
                VoucherGroupId = voucherGroupId,
                IsDone = false,
                Message = AppConstant.NotiMess.VOUCHER_DELETE_MESS + " " + AppConstant.NotiMess.PROCESSING_MESS,
                Type = AppConstant.NotiMess.Type.DELETE_VOUCHERS
            };

            Task.Run(() =>
            {
                if (!_cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation("DeleteVouchers is starting.");
                    notify.ProcessingVoucher(item: item);
                    _taskQueue.QueueBackgroundWorkItem(async token =>
                    {
                        try
                        {
                            voucherRepository.DeleteBulk(voucherGroupId: voucherGroupId);
                            item.Message = AppConstant.NotiMess.VOUCHER_DELETE_MESS + " " + AppConstant.NotiMess.PROCESSED_MESS;
                            item.IsDone = true;
                            await notify.ProcessedVoucher(item: item);
                        }
                        catch (Exception e)
                        {
                            _logger.LogInformation(e.Message);
                            item.Message = AppConstant.NotiMess.VOUCHER_DELETE_MESS + " " + AppConstant.NotiMess.ERROR_MESS;
                            item.IsDone = true;
                            await notify.ErrorProcess(item: item);
                        }
                    });
                }
            });
        }

        public void InsertVouchers(List<Voucher> vouchers, Guid voucherGroupId, Guid? promotionId)
        {
            var item = new VoucherNotiObj()
            {
                PromotionId = promotionId,
                VoucherGroupId = voucherGroupId,
                IsDone = false,
                Message = AppConstant.NotiMess.VOUCHER_INSERT_MESS + " " + AppConstant.NotiMess.PROCESSING_MESS,
                Type = AppConstant.NotiMess.Type.INSERT_VOUCHERS
            };
            Task.Run(() =>
            {
                if (!_cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation("InsertVouchers is starting.");
                    notify.ProcessingVoucher(item: item);
                    _taskQueue.QueueBackgroundWorkItem(async token =>
                    {
                        
                        try
                        {
                            await voucherRepository.InsertBulk(vouchers);
                            item.Message = AppConstant.NotiMess.VOUCHER_INSERT_MESS + " " + AppConstant.NotiMess.PROCESSED_MESS;
                            item.IsDone = true;
                            await notify.ProcessedVoucher(item: item);
                        }
                        catch (Exception e)
                        {
                            _logger.LogInformation(e.Message);
                            item.Message = AppConstant.NotiMess.VOUCHER_INSERT_MESS + " " + AppConstant.NotiMess.ERROR_MESS;
                            item.IsDone = true;
                            await notify.ErrorProcess(item: item);
                        }
                    });
                }
            });
        }


    }
}
