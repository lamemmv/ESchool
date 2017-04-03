using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Repositories
{
    public sealed class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly ObjectDbContext _dbContext;

        public Repository(ObjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public RepositoryQuery<T> Query
        {
            get
            {
                return new RepositoryQuery<T>(this);
            }
        }

        public async Task<T> FindAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.SingleOrDefaultAsync(predicate);
        }

        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }

        #region Create, Update, Delete, Commit

        public void Create(T entity)
        {
            var dbEntityEntry = DbSet.Add(entity);
            dbEntityEntry.State = EntityState.Added;
        }

        public async Task<int> CreateCommitAsync(T entity)
        {
            Create(entity);

            return await CommitAsync();
        }

        public void Update(T entity)
        {
            var dbEntityEntry = DbSet.Add(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        public async Task<int> UpdateCommitAsync(T entity)
        {
            Update(entity);

            return await CommitAsync();
        }

        public void Delete(T entity)
        {
            var dbEntity = DbSet.Remove(entity);
            dbEntity.State = EntityState.Deleted;
        }

        public async Task<int> DeleteCommitAsync(T entity)
        {
            Delete(entity);

            return await CommitAsync();
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = DbSet.Where(predicate);

            foreach (var entity in entities)
            {
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }
		}

        public async Task<int> DeleteCommitAsync(Expression<Func<T, bool>> predicate)
        {
            Delete(predicate);

            return await CommitAsync();
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        #endregion

        public IQueryable<T> GetQueryable(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includeProperties = null)
        {
            var query = DbSet.AsNoTracking();

            if (includeProperties != null)
            {
                includeProperties.ForEach(i => { query = query.Include(i); });
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }

        private DbSet<T> DbSet
        {
            get
            {
                return _dbContext.Set<T>();
            }
        }
    }
}
