using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Utils;
using ApplicationCore.Models.Store;
using Infrastructure.DTOs;
using AutoMapper;
using Infrastructure.UnitOrWork;
using Infrastructure.Repository;

namespace ApplicationCore.Services
{
    public class StoreService : BaseService<Store, StoreDto>, IStoreService
    {
        public StoreService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        protected override IGenericRepository<Store> _repository => _unitOfWork.StoreRepository;
    }

}



