using ApplicationCore.Request;
using ApplicationCore.Utils;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;


namespace ApplicationCore.Services
{
    public class TransactionService : BaseService<Transaction, TransactionDTO>, ITransactionService
    {
        private readonly IPromotionService _promotionService;
        private readonly IVoucherService _voucherService;
        private readonly IBrandService _brandService;
        private readonly IDeviceService _deviceService;
        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper, IDeviceService deviceService, IPromotionService promotionService, IVoucherService voucherService, IBrandService brandService) : base(unitOfWork, mapper)
        {
            _promotionService = promotionService;
            _voucherService = voucherService;
            _brandService = brandService;
            _deviceService = deviceService;
        }
        protected override IGenericRepository<Transaction> _repository => _unitOfWork.TransactionRepository;

        public async Task<Order> Checkout(Guid brandId, Order order, Guid deviceId)
        {
            var brand = await _brandService.GetByIdAsync(id: brandId);
            List<Promotion> promotionSetDiscounts = new List<Promotion>();
            if (brand != null)
            {
                var transactionId = Guid.NewGuid();
                if (order != null)
                {
                    if (order.Effects != null)
                    {

                        foreach (var effect in order.Effects)
                        {
                            if (effect.EffectType.Contains(AppConstant.EffectMessage.SetDiscount) || effect.EffectType.Contains(AppConstant.EffectMessage.AddGift))
                            {
                                var promotion = await _promotionService.GetByIdAsync(effect.PromotionId);
                                if (promotion == null || promotion.Status != (int)AppConstant.EnvVar.PromotionStatus.PUBLISH)
                                {
                                    //neu voucher dang apply vao order ma brand manager xoa promotion hoac change promotion status
                                    throw new ErrorObj(code: (int)AppConstant.ErrCode.Expire_Promotion, message: AppConstant.ErrMessage.Expire_Promotion, description: AppConstant.ErrMessage.Expire_Promotion);
                                }
                                promotionSetDiscounts.Add(_mapper.Map<Promotion>(promotion));
                                await CheckVoucher(order: order, deviceId: deviceId, transactionId: transactionId, (Guid)effect.PromotionTierId);
                            }
                        }
                        await AddTransaction(order: order, brandId: brandId, transactionId: transactionId);
                        // neu co setDiscount nhung ko them ap dung effect nao het

                        /*  if (promotionSetDiscounts.Count < 1)
                          {
                              //kiem tra co apply voucher nao ko
                              var listVoucher = await checkVoucher(order: order, deviceId: deviceId, transactionId: transactionId);
                              if(listVoucher != null)
                              return await AddTransaction(order: order, brandId: brandId, transactionId: transactionId);
                              else
                              return await AddTransaction(order: order, brandId: brandId, transactionId: transactionId);

                          }
                          else
                          {*/
                        try
                        {
                            /* var listVoucher = checkVoucher(order: order, deviceId: deviceId, transactionId: transactionId);*/
                        }
                        catch (ErrorObj e)
                        {
                            throw e;
                        }
                        foreach (var promotionSetDiscount in promotionSetDiscounts)
                        {
                            if (promotionSetDiscount.IsAuto)
                                return await AddTransactionWithPromo(order: order, brandId: brandId, transactionId: transactionId, promotionId: promotionSetDiscount.PromotionId);
                            if (!promotionSetDiscount.HasVoucher && !promotionSetDiscount.IsAuto)
                                return await AddTransactionWithPromo(order: order, brandId: brandId, transactionId: transactionId, promotionId: promotionSetDiscount.PromotionId);

                        }
                        //}
                    }
                    //neu ko co effect nao thi add transaction with no promotion
                    else return await AddTransaction(order: order, brandId: brandId, transactionId: transactionId);
                }
                else throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: AppConstant.ErrMessage.Order_Fail, description: AppConstant.ErrMessage.Order_Fail);
            }
            else
                throw new ErrorObj(code: (int)HttpStatusCode.NotFound, message: AppConstant.ErrMessage.Not_Found_Resource);
            return order;
        }
        private async Task<Order> AddTransaction(Order order, Guid brandId, Guid transactionId)
        {
            Order newOrder = order;
            newOrder.Effects = null;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(newOrder);
            var transaction = new Transaction()
            { BrandId = brandId, Id = transactionId, InsDate = DateTime.Now, UpdDate = DateTime.Now, TransactionJson = jsonString };
            _repository.Add(transaction);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                return order;
            }
            else
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: AppConstant.ErrMessage.Order_Fail, description: AppConstant.ErrMessage.Order_Fail);
        }
        private async Task<Order> AddTransactionWithPromo(Order order, Guid brandId, Guid transactionId, Guid promotionId)
        {
            var now = Common.GetCurrentDatetime();
            Order newOrder = order;
            newOrder.Effects = null;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(newOrder);
            var transaction = new Transaction()
            { BrandId = brandId, Id = transactionId, InsDate = now, UpdDate = now, TransactionJson = jsonString, PromotionId = promotionId };
            _repository.Add(transaction);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                return order;
            }
            else
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: AppConstant.ErrMessage.Order_Fail, description: AppConstant.ErrMessage.Order_Fail);
        }
        private async Task<List<Voucher>> CheckVoucher(Order order, Guid deviceId, Guid transactionId, Guid promotionTierId)
        {
            if (order.CustomerOrderInfo.Vouchers.Count > 0)
            {
                var device = await _deviceService.GetByIdAsync(deviceId);
                if (device != null)
                {
                    var appliedVoucher = await _voucherService.UpdateVoucherApplied(transactionId: transactionId, order: order.CustomerOrderInfo, storeId: device.StoreId, promotionTierId);
                    return appliedVoucher;
                }
            }
            return null;
        }
    }
}
