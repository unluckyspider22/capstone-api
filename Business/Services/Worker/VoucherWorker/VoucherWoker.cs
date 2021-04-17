using ApplicationCore.Utils;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Worker
{
    public interface IVoucherWorker
    {
        public void InsertVouchers(VoucherGroupDto voucherDto, bool isAddMore = false, List<Voucher> vouchersAdd = null);
        public void DeleteVouchers(Guid voucherGroupId);
        public List<Voucher> GenerateVoucher(VoucherGroupDto dto, bool isAddMore = false);
    }
    public class VoucherWorker : IVoucherWorker
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger _logger;
        private readonly CancellationToken _cancellationToken;
        private readonly IVoucherRepository voucherRepository = new VoucherRepositoryImp();
        private readonly IVoucherNotify notify = new VoucherNotify();
        protected readonly IMapper _mapper;

        public VoucherWorker(IBackgroundTaskQueue taskQueue,
            ILogger<VoucherWorker> logger,
            IHostApplicationLifetime applicationLifetime, IMapper mapper)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _cancellationToken = applicationLifetime.ApplicationStopping;
            _mapper = mapper;
        }

        public void DeleteVouchers(Guid voucherGroupId)
        {
            // Task item để notify cho client
            var item = new VoucherNotiObj()
            {
                //PromotionId = promotionId,
                VoucherGroupId = voucherGroupId,
                IsDone = false,
                Message = AppConstant.NotiMess.VOUCHER_DELETE_MESS + " " + AppConstant.NotiMess.PROCESSING_MESS,
                Type = AppConstant.NotiMess.Type.DELETE_VOUCHERS
            };

            // Chạy luồng
            Task.Run(() =>
             {
                 if (!_cancellationToken.IsCancellationRequested)
                 {
                     _logger.LogInformation("DeleteVouchers is starting.");
                     // Gửi notify processing task
                     //notify.ProcessingVoucher(item: item);
                     _taskQueue.QueueBackgroundWorkItem(async token =>
                     {
                         try
                         {
                             // Delete voucher
                             await voucherRepository.DeleteBulk(voucherGroupId: voucherGroupId);
                             item.Message = AppConstant.NotiMess.VOUCHER_DELETE_MESS + " " + AppConstant.NotiMess.PROCESSED_MESS;
                             item.IsDone = true;
                             // Gửi notify hoàn thành task
                             //await notify.ProcessedVoucher(item: item);
                         }
                         catch (Exception e)
                         {
                             _logger.LogInformation(e.Message);
                             item.Message = AppConstant.NotiMess.VOUCHER_DELETE_MESS + " " + AppConstant.NotiMess.ERROR_MESS;
                             item.IsDone = true;
                             // Gửi notify lỗi
                             //await notify.ErrorProcess(item: item);
                         }
                     });
                 }
             });
        }

        public void InsertVouchers(VoucherGroupDto dto, bool isAddMore, List<Voucher> vouchersAdd)
        {
            // Param để insert
            Guid voucherGroupId = dto.VoucherGroupId;
            Guid? promotionId = dto.PromotionId;

            // Tạo voucher

            // Task item để notify cho client
            var item = new VoucherNotiObj()
            {
                PromotionId = promotionId,
                VoucherGroupId = voucherGroupId,
                IsDone = false,
                Message = AppConstant.NotiMess.VOUCHER_INSERT_MESS + " " + AppConstant.NotiMess.PROCESSING_MESS,
                Type = AppConstant.NotiMess.Type.INSERT_VOUCHERS
            };

            item.Message = AppConstant.NotiMess.VOUCHER_GENERATE_MESS + " " + AppConstant.NotiMess.PROCESSING_MESS;
            notify.GeneratingVoucher(item);
            _logger.LogInformation(">>>>>>Start generate: " + DateTime.Now.ToString("HH:mm:ss"));
            List<Voucher> vouchers = vouchersAdd;
            if (!isAddMore && vouchersAdd == null)
            {
                vouchers = GenerateDistinctVoucher(dto);
            }
            _logger.LogInformation(">>>>>>End generate: " + DateTime.Now.ToString("HH:mm:ss"));
            item.Message = AppConstant.NotiMess.VOUCHER_GENERATE_MESS + " " + AppConstant.NotiMess.PROCESSED_MESS;
            notify.GeneratedVoucher(item);

            // Chạy luồng
            Task.Run(() =>
            {
                if (!_cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation("InsertVouchers is starting.");
                    // Gửi notify processing task
                    notify.ProcessingVoucher(item: item);
                    _taskQueue.QueueBackgroundWorkItem(async token =>
                    {
                        try
                        {
                            _logger.LogInformation(">>>>>>Start insert: " + DateTime.Now.ToString("HH:mm:ss"));
                            // Insert vouchers 
                            await voucherRepository.InsertBulk(vouchers);
                            _logger.LogInformation(">>>>>>End insert: " + DateTime.Now.ToString("HH:mm:ss"));
                            item.Message = AppConstant.NotiMess.VOUCHER_INSERT_MESS + " " + AppConstant.NotiMess.PROCESSED_MESS;
                            item.IsDone = true;
                            // Gửi notify hoàn thành task
                            await notify.ProcessedVoucher(item: item);
                        }
                        catch (Exception e)
                        {
                            _logger.LogInformation(e.Message);
                            item.Message = AppConstant.NotiMess.VOUCHER_INSERT_MESS + " " + AppConstant.NotiMess.ERROR_MESS;
                            item.IsDone = true;
                            // Gửi notify lỗi
                            await notify.ErrorProcess(item: item);
                        }
                    });
                }
            });
        }

        private List<Voucher> GenerateDistinctVoucher(VoucherGroupDto dto)
        {
            var now = Common.GetCurrentDatetime();
            var vouchers = new List<Voucher>();
            var quantity = dto.Quantity;
            var codes = GenerateVoucher(dto).Select(el => el.VoucherCode).Distinct(StringComparer.CurrentCulture);

            while (codes.Count() < quantity)
            {
                var remainQuantity = quantity - codes.Count();
                dto.Quantity = remainQuantity;
                var temp = GenerateVoucher(dto).Select(el => el.VoucherCode).Distinct(StringComparer.CurrentCulture);
                codes = codes.Union(temp);
            }
            for (int i = 0; i < codes.Count(); i++)
            {
                var v = new Voucher()
                {
                    VoucherGroupId = dto.VoucherGroupId,
                    VoucherCode = codes.ElementAt(i),
                    InsDate = now,
                    UpdDate = now,
                    Index = i,
                };
                vouchers.Add(v);
            }
            return vouchers;

        }
        #region generate voucher
        public List<Voucher> GenerateVoucher(VoucherGroupDto dto, bool isAddMore = false)
        {
            return _mapper.Map<List<Voucher>>(GenerateBulkCodeVoucher(dto, isAddMore));
        }

        private List<VoucherDto> GenerateBulkCodeVoucher(VoucherGroupDto dto, bool isAddMore = false)
        {
            List<VoucherDto> result = new List<VoucherDto>();
            var now = Common.GetCurrentDatetime();
            for (var i = 0; i < dto.Quantity; i++)
            {
                VoucherDto voucher = new VoucherDto();
                string randomVoucher = RandomString(dto.Charset, dto.CustomCharset, dto.CodeLength);
                voucher.VoucherCode = dto.Prefix + randomVoucher + dto.Postfix;
                voucher.VoucherGroupId = dto.VoucherGroupId;
                if (isAddMore)
                {
                    voucher.Index = -1;
                }
                else
                {
                    voucher.Index = i + 1;
                }
                voucher.InsDate = now;
                voucher.UpdDate = now;
                result.Add(voucher);
            }
            return result;
        }
        private Random Random = new Random();
        private string RandomString(string charset, string customCode, int length)
        {
            if (length == 0)
            {
                length = 10;
            }
            string randomCode = "";
            string chars = "";
            switch (charset)
            {
                case AppConstant.EnvVar.CharsetType.ALPHABETIC:
                    chars = AppConstant.EnvVar.CharsetChars.ALPHABETIC;

                    randomCode = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
                    break;
                case AppConstant.EnvVar.CharsetType.ALPHANUMERIC:
                    chars = AppConstant.EnvVar.CharsetChars.ALPHANUMERIC;

                    randomCode = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
                    break;
                case AppConstant.EnvVar.CharsetType.ALPHABETIC_UPERCASE:
                    chars = AppConstant.EnvVar.CharsetChars.ALPHABETIC_UPERCASE;

                    randomCode = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
                    break;
                case AppConstant.EnvVar.CharsetType.ALPHABETIC_LOWERCASE:
                    chars = AppConstant.EnvVar.CharsetChars.ALPHABETIC_LOWERCASE;

                    randomCode = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
                    break;
                case AppConstant.EnvVar.CharsetType.NUMBERS:
                    chars = AppConstant.EnvVar.CharsetChars.NUMBERS;

                    randomCode = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
                    break;
                case AppConstant.EnvVar.CharsetType.CUSTOM:
                    chars = customCode;
                    randomCode = new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
                    break;
            }
            return randomCode;
        }

        #endregion
    }
}
