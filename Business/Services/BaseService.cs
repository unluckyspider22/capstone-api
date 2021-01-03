using AutoMapper;
using Infrastructure.Repository;
using Infrastructure.UnitOrWork;
using System;
using System.Collections.Generic;
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
            var entity = _mapper.Map<TEntity>(dto);

            _repository.Add(entity);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            if (id != null)
            {
                _repository.Delete(id);
            }

            return await _unitOfWork.SaveAsync() > 0;
             
        }

        public async Task<IEnumerable<TEntity>> GetAsync(int pageIndex = 0, int pageSize = 0, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            return await _repository.Get(pageIndex, pageSize, filter, orderBy, includeProperties);
        }

        public virtual async Task<TDto> GetByIdAsync(Guid id)
        {
            if (id != null)
            {
                return _mapper.Map<TDto>(await _repository.GetById(id));
            }
            return null; 
        }

        public virtual async Task<TDto> UpdateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _repository.Update(entity);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<bool> HideAsync(Guid id, string value)
        {
            if (id != null)
            {
                _repository.Hide(id, value);
            }
            return await _unitOfWork.SaveAsync() > 0;
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return _repository.CountAsync(filter);
        }
    }
}
