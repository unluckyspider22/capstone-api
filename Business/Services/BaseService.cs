﻿using AutoMapper;
using Infrastructure.DTOs;
using Infrastructure.Helper;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public abstract class BaseService<TEntity, TDto> : IBaseService<TEntity, TDto>
        where TEntity : class
        where TDto : class

    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected abstract IGenericRepository<TEntity> _repository { get; }
        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public virtual async Task<TDto> CreateAsync(TDto dto)
        {
            try
            {   
                var entity = _mapper.Map<TEntity>(dto);
                _repository.Add(entity);
                await _unitOfWork.SaveAsync();
                return _mapper.Map<TDto>(entity);
            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at CreateAsync: \n" + e.Message);
                throw new ErrorObj(code: 500,message: "Oops !!! Something Wrong. Try Again.",description: "Internal Server Error");
            }            
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
           
            try
            {   
                _repository.Delete(id);             
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at DeleteAsync: \n" + e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }
         

        }

        public async Task<GenericRespones<TEntity>> GetAsync(int pageIndex = 0, int pageSize = 0, Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {                   
            try
            {
                var list = await _repository.Get(pageIndex, pageSize, filter, orderBy, includeProperties);
                var totalItem = await _repository.CountAsync();
                MetaData metadata  = new MetaData(pageIndex: pageIndex, pageSize: pageSize, totalItems: totalItem);

                GenericRespones<TEntity> result = new GenericRespones<TEntity>(data: list.ToList(), metadata: metadata);
                return result;
            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at GetAsync: \n" + e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }
        }

        public virtual async Task<TDto> GetByIdAsync(Guid id)
        {
            try
            {             
                    return _mapper.Map<TDto>(await _repository.GetById(id));
            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at GetByIdAsync: \n" + e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }
        }

        public virtual async Task<TDto> UpdateAsync(TDto dto)
        {        
            try
            {
                var entity = _mapper.Map<TEntity>(dto);
                _repository.Update(entity);
                await _unitOfWork.SaveAsync();
                return _mapper.Map<TDto>(entity);
            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at UpdateAsync: \n" + e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }
        }

        public virtual async Task<bool> HideAsync(Guid id, string value)
        {
           
            try
            {
                if (id != null)
                {
                    _repository.Hide(id, value);
                }
                return await _unitOfWork.SaveAsync() > 0;
            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at HideAsync: \n" + e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            try
            {
                return _repository.CountAsync(filter);
            }
            catch (Exception e)
            {
                //chạy bằng debug mode để xem log
                Debug.WriteLine("\n\nError at CountAsync: \n" + e.Message);
                throw new ErrorObj(code: 500, message: "Oops !!! Something Wrong. Try Again.");
            }
            
        }
    }
}
