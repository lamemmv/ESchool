using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Services
{
    public abstract class BaseService<T> : IService<T> where T : class, new()
    {
        protected readonly ObjectDbContext _dbContext;

        public BaseService(ObjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<T> DbSet
        {
            get
            {
                return _dbContext.Set<T>();
            }
        }

        public IQueryable<T> DbSetNoTracking
        {
            get
            {
                return DbSet.AsNoTracking();
            }
        }

        public virtual async Task<T> FindAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }
        
        #region Create, Update, Delete, Commit

        public virtual async Task<ErrorCode> CreateAsync(T entity)
        {
            var dbEntityEntry = DbSet.Add(entity);

            return await CommitAsync();
        }

        public abstract Task<ErrorCode> UpdateAsync(int id, T entity);

        public virtual async Task<ErrorCode> DeleteAsync(T entity)
        {
            var dbEntity = DbSet.Remove(entity);

            return await CommitAsync();
        }

        public virtual async Task<ErrorCode> DeleteAsync(int id)
        {
            T entity = await DbSet.FindAsync(id);

            if (entity == null)
            {
                return ErrorCode.NotFound;
            }

            return await DeleteAsync(entity);
        }

        public async Task<ErrorCode> CommitAsync()
        {
            int effectedRows = await _dbContext.SaveChangesAsync();

            return ErrorCode.Success;
        }

        #endregion
    }
}
