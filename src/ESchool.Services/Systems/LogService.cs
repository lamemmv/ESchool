using System;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.Paginations;
using ESchool.Domain.Entities.Systems;
using ESchool.Domain.Enums;

namespace ESchool.Services.Systems
{
    public class LogService : BaseService<Log>, ILogService
    {
        public LogService(ObjectDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<IPagedList<Log>> GetListAsync(DateTime fromData, DateTime toDate, string level, int page, int size)
        {
            var logs = DbSetNoTracking
                .Where(l =>
                    (l.Logged.Date >= fromData.Date && l.Logged.Date <= toDate.Date) ||
                    string.Equals(l.Level, level, StringComparison.OrdinalIgnoreCase))
                .OrderBy(l => l.Id);

            return await logs.GetListAsync(page, size);
        }

        public override async Task<ErrorCode> UpdateAsync(int id, Log entity)
        {
            return await Task.FromResult(ErrorCode.BadRequest);
        }

        public async Task<ErrorCode> DeleteAsync(int[] ids)
        {
            var logs = DbSet.Where(l => ids.Contains(l.Id));

            if (!logs.Any())
            {
                return ErrorCode.NotFound;
            }

            DbSet.RemoveRange(logs);

            return await CommitAsync();
        }
    }
}
