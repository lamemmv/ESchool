using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ESchool.Domain.Models.Paginations;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Repositories
{
    public sealed class RepositoryQuery<T> where T : class, new()
    {
        private readonly IRepository<T> _repository;
        private readonly List<Expression<Func<T, object>>> _includeProperties;

        private Expression<Func<T, bool>> _filter;
        private Func<IQueryable<T>, IOrderedQueryable<T>> _orderByQuerable;

        public RepositoryQuery(IRepository<T> repository)
        {
            _repository = repository;
            _includeProperties = new List<Expression<Func<T, object>>>();
        }

        public RepositoryQuery<T> Filter(Expression<Func<T, bool>> filter)
        {
            _filter = filter;
            return this;
        }

        public RepositoryQuery<T> Sort(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            _orderByQuerable = orderBy;
            return this;
        }

        public RepositoryQuery<T> Include(Expression<Func<T, object>> expression)
        {
            _includeProperties.Add(expression);
            return this;
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repository.GetSingleAsync(predicate);
        }

        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repository.GetFirstAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetListAsync(int page, int size)
        {
            int totalItems = await _repository.GetQueryable(_filter).CountAsync();

            if (totalItems == 0)
            {
                return new PagedList<T>(Enumerable.Empty<T>(), page, size, totalItems);
            }

            var pagedList = await _repository
                .GetQueryable(_filter, _orderByQuerable, _includeProperties)
                .Skip((page - 1) * size).Take(size)
                .AsNoTracking()
                .ToListAsync();

            return pagedList.ToPagedList(page, size, totalItems);
        }

        public async Task<IEnumerable<T>> GetListAsync()
        {
            return await _repository
                .GetQueryable(_filter, _orderByQuerable, _includeProperties)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
