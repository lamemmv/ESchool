using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ESchool.Data.Paginations;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Repositories
{
    public sealed class QueryRepository<T> where T : class, new()
    {
        private readonly IQueryable<T> _source;
        private readonly IList<Expression<Func<T, object>>> _includeProperties;

        private Expression<Func<T, bool>> _filter;
        private Func<IQueryable<T>, IOrderedQueryable<T>> _orderByQuerable;

        public QueryRepository(IQueryable<T> source)
        {
            _source = source;
            _includeProperties = new List<Expression<Func<T, object>>>();
        }

        public QueryRepository<T> Filter(Expression<Func<T, bool>> filter)
        {
            _filter = filter;

            return this;
        }

        public QueryRepository<T> Sort(Func<IQueryable<T>, IOrderedQueryable<T>> orderByQuerable)
        {
            _orderByQuerable = orderByQuerable;

            return this;
        }

        public QueryRepository<T> Include(Expression<Func<T, object>> expression)
        {
            _includeProperties.Add(expression);

            return this;
        }

        public async Task<T> GetSingleAsync()
        {
            return await GetQueryable().SingleOrDefaultAsync();
        }

        public async Task<IPagedList<T>> ToPagedListAsync(int page, int size)
        {
            IQueryable<T> queryable = GetQueryable();

            int totalItems = queryable.Count();

            if (totalItems == 0)
            {
                return new PagedList<T>(Enumerable.Empty<T>(), page, size, totalItems);
            }

            var pagedList = await queryable.Skip((page - 1) * size).Take(size).ToListAsync();

            return pagedList.ToPagedList(page, size, totalItems);
        }

        public async Task<List<T>> ToListAsync()
        {
            return await GetQueryable().ToListAsync();
        }

        private IQueryable<T> GetQueryable()
        {
            IQueryable<T> query = _source;

            if (_includeProperties != null)
            {
                foreach (var includeProperty in _includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (_filter != null)
            {
                query = query.Where(_filter);
            }

            if (_orderByQuerable != null)
            {
                query = _orderByQuerable(query);
            }

            return query;
        }
    }
}
