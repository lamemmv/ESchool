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
        private IQueryable<T> _queryable;
        private readonly List<Expression<Func<T, object>>> _includeProperties;

        private Expression<Func<T, bool>> _filter;
        private Func<IQueryable<T>, IOrderedQueryable<T>> _orderByQuerable;

        public RepositoryQuery(IQueryable<T> queryable)
        {
            _queryable = queryable;
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

        public async Task<T> GetSingleAsync()
        {
            return await GetQueryable().SingleOrDefaultAsync();
        }

        public async Task<T> GetFirstAsync()
        {
            return await GetQueryable().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetListAsync(int page, int size)
        {
            var query = GetQueryable();

            int totalItems = await query.CountAsync();

            if (totalItems == 0)
            {
                return new PagedList<T>(Enumerable.Empty<T>(), page, size, totalItems);
            }

            var pagedList = await query
                .Skip((page - 1) * size).Take(size)
                .ToListAsync();

            return pagedList.ToPagedList(page, size, totalItems);
        }

        public async Task<IEnumerable<T>> GetListAsync()
        {
            return await GetQueryable().ToListAsync();
        }

        private IQueryable<T> GetQueryable()
        {
            if (_includeProperties != null)
            {
                _includeProperties.ForEach(i => { _queryable = _queryable.Include(i); });
            }

            if (_filter != null)
            {
                _queryable = _queryable.Where(_filter);
            }

            if (_orderByQuerable != null)
            {
                _queryable = _orderByQuerable(_queryable);
            }

            return _queryable;
        }
    }
}
