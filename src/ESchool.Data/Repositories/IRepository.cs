using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ESchool.Data.Repositories
{
    public interface IRepository<T> where T : class, new()
    {
        RepositoryQuery<T> Query { get; }

        Task<T> FindAsync(object id);

        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetQueryable(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includeProperties = null);

        void Create(T entity);

        Task<int> CreateCommitAsync(T entity);

        void Update(T entity);

        Task<int> UpdateCommitAsync(T entity);

        void Delete(T entity);

        Task<int> DeleteCommitAsync(T entity);

        void Delete(Expression<Func<T, bool>> predicate);

        Task<int> DeleteCommitAsync(Expression<Func<T, bool>> predicate);

        Task<int> CommitAsync();
    }
}
