using ApplicationCore.Services;
using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ApplicationCore.Services
{
    public class RoleService : BaseService<RoleEntity, RoleDto>, IRoleService
    {
        public RoleService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }

        protected override IGenericRepository<RoleEntity> _repository => _unitOfWork.RoleEntityRepository;
    }
}
