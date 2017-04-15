using System.Linq;
using System.Threading.Tasks;
using ESchool.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Services
{
    public interface IService<T> where T : class, new()
    {
        DbSet<T> DbSet { get; }

        IQueryable<T> DbSetNoTracking { get; }

        Task<T> FindAsync(int id);

        Task<ErrorCode> CreateAsync(T entity);

        Task<ErrorCode> UpdateAsync(int id, T entity);

        Task<ErrorCode> DeleteAsync(T entity);

        Task<ErrorCode> DeleteAsync(int id);

        Task<ErrorCode> CommitAsync();
    }
}
