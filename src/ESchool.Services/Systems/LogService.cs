using System;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.Paginations;
using ESchool.Domain.Entities.Systems;
using ESchool.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        public async Task<IPagedList<Log>> GetListAsync(DateTime fromData, DateTime toDate, string level, int page, int size)
        {
            var entities = Logs.AsNoTracking()
                .Where(l =>
                    (l.Logged.Date >= fromData.Date && l.Logged.Date <= toDate.Date) ||
                    string.Equals(l.Level, level, StringComparison.OrdinalIgnoreCase))
                .OrderBy(l => l.Id);

            return await entities.GetListAsync(page, size);
        }

        public async Task<ErrorCode> DeleteAsync(int id)
        {
            var entity = await FindAsync(id);

            if (entity == null)
            {
                return ErrorCode.NotFound;
            }

            Logs.Remove(entity);

            return await CommitAsync();
        }

        public async Task<ErrorCode> DeleteAsync(int[] ids)
        {
            var dbSet = Logs;
            var logs = dbSet.Where(l => ids.Contains(l.Id));

            if (!logs.Any())
            {
                return ErrorCode.NotFound;
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
