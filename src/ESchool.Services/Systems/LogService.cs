using System;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.Entities.Systems;
using ESchool.Data.Paginations;
using ESchool.Services.Exceptions;
using ESchool.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Services.Systems
{
    public class LogService : BaseService, ILogService
    {
        public LogService(ObjectDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<Log> FindAsync(int id)
        {
            return await Logs.FindAsync(id);
        }

        public async Task<IPagedList<Log>> GetListAsync(DateTime fromDate, DateTime toDate, string level, int page, int size)
        {
            DateTime startDate = fromDate.StartOfDay();
            DateTime endDate = toDate.EndOfDay();

            return await Logs.AsNoTracking()
                .Where(l =>
                    (l.Logged >= startDate && l.Logged <= endDate) ||
                    string.Equals(l.Level, level, StringComparison.OrdinalIgnoreCase))
                .OrderBy(l => l.Id)
                .GetListAsync(page, size);
        }

        public async Task<int> DeleteAsync(int id)
        {
            Log entity = await FindAsync(id);

            if (entity == null)
            {
                throw new EntityNotFoundException(id, nameof(Log));
            }

            Logs.Remove(entity);

            return await CommitAsync();
        }

        public async Task<int> DeleteAsync(int[] ids)
        {
            var dbSet = Logs;
            var logs = dbSet.Where(l => ids.Contains(l.Id));

            if (!logs.Any())
            {
                throw new EntityNotFoundException($"Logs not found. Ids = {string.Join(",", ids)}");
            }

            dbSet.RemoveRange(logs);

            return await CommitAsync();
        }

        private DbSet<Log> Logs
        {
            get
            {
                return _dbContext.Set<Log>();
            }
        }
    }
}
