using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ESchool.Data.Repositories
{
    public interface IRepository<T> where T : class, new()
    {
        RepositoryQuery<T> Query { get; }

        RepositoryQuery<T> QueryNoTracking { get; }

        Task<T> FindAsync(object id);

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
