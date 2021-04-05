using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Request;
using Infrastructure.DTOs;
using Infrastructure.Models;

namespace ApplicationCore.Services
{
    public interface ITransactionService : IBaseService<Transaction, TransactionDTO>
    {
        public Task<OrderResponseModel> Checkout( Guid brandId, OrderResponseModel order);
    }
}
