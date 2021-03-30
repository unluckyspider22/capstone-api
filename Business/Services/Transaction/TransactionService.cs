using ApplicationCore.Request;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Net;
using System.Threading.Tasks;


namespace ApplicationCore.Services
{
    public class TransactionService : BaseService<Transaction, TransactionDTO>, ITransactionService
    {
        private readonly IPromotionService _promotionService;
        private readonly IVoucherService _voucherService;
        private readonly IBrandService _brandService;
        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper, IPromotionService promotionService, IVoucherService voucherService, IVoucherGroupService voucherGroupService) : base(unitOfWork, mapper)
        {
            this._promotionService = promotionService;
            this._voucherService = voucherService;
        }
        protected override IGenericRepository<Transaction> _repository => _unitOfWork.TransactionRepository;

        public async Task<OrderResponseModel> Checkout(Guid brandId, OrderResponseModel order)
        {
            var brand = _brandService.GetByIdAsync(id: brandId);
            if (brand != null)
            {
                if (order != null)
                {
                    if (order.Effects != null)
                    {
                        foreach (var effect in order.Effects)
                        {
                            var promotion = await _promotionService.GetByIdAsync(effect.PromotionId);
                            if (promotion == null || !promotion.Status.Equals(AppConstant.EnvVar.PromotionStatus.PUBLISH))
                            {
                                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: AppConstant.ErrMessage.Expire_Promotion, description: AppConstant.ErrMessage.Expire_Promotion);
                            }
                        }
                        var appliedVoucher = await _voucherService.UpdateVoucherApplied(order: order.CustomerOrderInfo);
                        return await AddTransaction(order: order, brandId: brandId);
                    }
                    else return await AddTransaction(order: order, brandId: brandId);

                }
            }
            else
                throw new ErrorObj(code: (int)HttpStatusCode.BadRequest, message: "Brand does not exist !", description: "Brand does not exist !");
            return null;
        }
        private async Task<OrderResponseModel> AddTransaction(OrderResponseModel order, Guid brandId)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(order);
            var transaction = new Transaction()
            { BrandId = brandId, Id = Guid.NewGuid(), InsDate = DateTime.Now, UpdDate = DateTime.Now, TransactionJson = jsonString };
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
