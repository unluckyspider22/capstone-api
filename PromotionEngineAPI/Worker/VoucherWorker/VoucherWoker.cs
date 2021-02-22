using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository.Voucher;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PromotionEngineAPI.Worker
{
    public interface IVoucherWorker
    {
        public void InsertVouchers(VoucherGroupDto voucherDto);
        public void DeleteVouchers(Guid voucherGroupId, Guid? promotionId);
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

        public void DeleteVouchers(Guid voucherGroupId, Guid? promotionId)
        {
            // Task item để notify cho client
            var item = new VoucherNotiObj()
            {
                PromotionId = promotionId,
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
                    notify.ProcessingVoucher(item: item);
                    _taskQueue.QueueBackgroundWorkItem(async token =>
                    {
                        try
                        {
                            // Delete voucher
                            voucherRepository.DeleteBulk(voucherGroupId: voucherGroupId);
                            item.Message = AppConstant.NotiMess.VOUCHER_DELETE_MESS + " " + AppConstant.NotiMess.PROCESSED_MESS;
                            item.IsDone = true;
                            // Gửi notify hoàn thành task
                            await notify.ProcessedVoucher(item: item);
                        }
                        catch (Exception e)
                        {
                            _logger.LogInformation(e.Message);
                            item.Message = AppConstant.NotiMess.VOUCHER_DELETE_MESS + " " + AppConstant.NotiMess.ERROR_MESS;
                            item.IsDone = true;
                            // Gửi notify lỗi
                            await notify.ErrorProcess(item: item);
                        }
                    });
                }
            });
        }

        public void InsertVouchers(VoucherGroupDto dto)
        {
            // Param để insert
            Guid voucherGroupId = dto.VoucherGroupId;
            Guid? promotionId = dto.PromotionId;

            // Tạo voucher
            List<Voucher> vouchers = GenerateVoucher(dto);

            // Task item để notify cho client
            var item = new VoucherNotiObj()
            {
                PromotionId = promotionId,
                VoucherGroupId = voucherGroupId,
                IsDone = false,
                Message = AppConstant.NotiMess.VOUCHER_INSERT_MESS + " " + AppConstant.NotiMess.PROCESSING_MESS,
                Type = AppConstant.NotiMess.Type.INSERT_VOUCHERS
            };

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
                            // Insert vouchers 
                            await voucherRepository.InsertBulk(vouchers);
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
        #region generate voucher
        private List<Voucher> GenerateVoucher(VoucherGroupDto dto)
        {
            List<VoucherDto> voucherDto = new List<VoucherDto>();
            if (dto.VoucherType.Equals(AppConstant.EnvVar.VoucherType.BULK_CODE))
            {
                voucherDto = GenerateBulkCodeVoucher(dto);
            }
            else
            {
                voucherDto = GenerateStandaloneVoucher(dto);
            }
            return _mapper.Map<List<Voucher>>(voucherDto);
        }
        private List<VoucherDto> GenerateStandaloneVoucher(VoucherGroupDto dto)
        {
            List<VoucherDto> result = new List<VoucherDto>();
            VoucherDto voucher = new VoucherDto
            {
                VoucherCode = dto.Prefix + dto.CustomCode + dto.Postfix,
                VoucherGroupId = dto.VoucherGroupId
            };
            result.Add(voucher);
            return result;
        }

        private List<VoucherDto> GenerateBulkCodeVoucher(VoucherGroupDto dto)
        {
            List<VoucherDto> result = new List<VoucherDto>();
            if (!dto.IsLimit)
            {
                for (var i = 0; i < dto.Quantity; i++)
                {
                    VoucherDto voucher = new VoucherDto();
                    string randomVoucher = RandomString(dto.Charset, dto.CustomCharset, dto.CodeLength);
                    voucher.VoucherCode = dto.Prefix + randomVoucher + dto.Postfix;
                    voucher.VoucherGroupId = dto.VoucherGroupId;
                    result.Add(voucher);
                }
            }
            else
            {
                VoucherDto voucher = new VoucherDto();
                string randomVoucher = RandomString(dto.Charset, dto.CustomCharset, dto.CodeLength);
                voucher.VoucherCode = dto.Prefix + randomVoucher + dto.Postfix;
                voucher.VoucherGroupId = dto.VoucherGroupId;
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
