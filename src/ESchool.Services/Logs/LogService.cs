using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data.Paginations;
using ESchool.Data.Repositories;
using ESchool.DomainModels.Entities.Logs;

namespace ESchool.Services.Logs
{
	public class LogService : ILogService
    {
        private readonly IRepository<Log> _logRepository;

        public LogService(IRepository<Log> logRepository)
        {
            _logRepository = logRepository;
        }

		public async Task<Log> GetSingleAsync(int id)
		{
			return await _logRepository.GetSingleAsync(id);
		}

		public async Task<IPagedList<Log>> GetLogsAsync(DateTime fromData, DateTime toDate, string level, int page, int size)
        {
            return await _logRepository.QueryNoTraking()
                .Filter(l => (l.Logged.Date >= fromData.Date && l.Logged.Date <= toDate.Date) ||
                    string.Equals(l.Level, level, StringComparison.OrdinalIgnoreCase))
                .Sort(o => o.OrderByDescending(l => l.Id))
                .ToPagedListAsync(page, size);
        }

		public async Task<int> DeleteAsync(Log entity)
		{
			return await _logRepository.DeleteCommitAsync(entity);
		}

		public async Task<int> DeleteAsync(IEnumerable<int> ids)
		{
			return await _logRepository.DeleteCommitAsync(l => ids.Contains(l.Id));
		}
	}
}
