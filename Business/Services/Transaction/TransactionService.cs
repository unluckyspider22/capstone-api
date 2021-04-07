using ApplicationCore.Request;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
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
        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper,IDeviceService deviceService, IPromotionService promotionService, IVoucherService voucherService, IBrandService brandService) : base(unitOfWork, mapper)
        {
            _promotionService = promotionService;
            _voucherService = voucherService;
            _brandService = brandService;
            _deviceService = deviceService;
        }
        protected override IGenericRepository<Transaction> _repository => _unitOfWork.TransactionRepository;

        public async Task<OrderResponseModel> Checkout(Guid brandId, OrderResponseModel order,Guid deviceId)
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
                            if (effect.EffectType.Equals("setDiscount"))
                            {
                                var promotion = await _promotionService.GetByIdAsync(effect.PromotionId);
                                if (promotion == null || promotion.Status != (int)AppConstant.EnvVar.PromotionStatus.PUBLISH)
                                {
                                    //neu voucher dang apply vao order ma brand manager xoa promotion hoac change promotion status
                                    throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Expire_Promotion, description: AppConstant.ErrMessage.Expire_Promotion);
                                }
                                promotionSetDiscounts.Add(_mapper.Map<Promotion>(promotion));
                            }
                        }
                        if (promotionSetDiscounts.Count < 1)
                        {
                            // neu co effect nhung ko them ap dung effect nao het
                            return await AddTransaction(order: order, brandId: brandId, transactionId: transactionId);
                        }
                        else
                        {
                            try
                            {
                                if (order.CustomerOrderInfo.Vouchers != null)
                                {
                                    var device = await _deviceService.GetByIdAsync(deviceId);
                                    if(device != null)
                                    {
                                        var appliedVoucher = await _voucherService.UpdateVoucherApplied(transactionId: transactionId, order: order.CustomerOrderInfo, storeId: device.StoreId);
                                    }
                                }
                            }
                            catch (ErrorObj e)
                            {
                                throw e;
                            }
                            foreach (var promotionSetDiscount in promotionSetDiscounts)
                            {
                                if (promotionSetDiscount.IsAuto)
                                    await AddTransactionWithPromo(order: order, brandId: brandId, transactionId: transactionId, promotionId: promotionSetDiscount.PromotionId);
                                if (!promotionSetDiscount.HasVoucher && !promotionSetDiscount.IsAuto)
                                    await AddTransactionWithPromo(order: order, brandId: brandId, transactionId: transactionId, promotionId: promotionSetDiscount.PromotionId);

                            }
                            return await AddTransaction(order: order, brandId: brandId, transactionId: transactionId);
                        }
                    }
                    //neu ko co effect nao thi add transaction with no promotion
                    else return await AddTransaction(order: order, brandId: brandId, transactionId: transactionId);
                }
                else throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: "Order failed !", description: "Order failed !");
            }
            else
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: "Brand does not exist !", description: "Brand does not exist !");
        }
        private async Task<OrderResponseModel> AddTransaction(OrderResponseModel order, Guid brandId, Guid transactionId)
        {

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(order);
            var transaction = new Transaction()
            { BrandId = brandId, Id = transactionId, InsDate = DateTime.Now, UpdDate = DateTime.Now, TransactionJson = jsonString };
            _repository.Add(transaction);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                return order;
            }
            else
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: "Order failed !", description: "Order failed !");
        }
        private async Task<OrderResponseModel> AddTransactionWithPromo(OrderResponseModel order, Guid brandId, Guid transactionId, Guid promotionId)
        {

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(order);
            var transaction = new Transaction()
            { BrandId = brandId, Id = transactionId, InsDate = DateTime.Now, UpdDate = DateTime.Now, TransactionJson = jsonString, PromotionId = promotionId };
            _repository.Add(transaction);
            if (await _unitOfWork.SaveAsync() > 0)
            {
                return order;
            }
            else
                throw new ErrorObj(code: (int)HttpStatusCode.InternalServerError, message: "Order failed !", description: "Order failed !");
        }
    }
}
