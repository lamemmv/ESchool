using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data.Repositories;
using ESchool.Domain.Entities.Systems;

namespace ESchool.Services.Systems
{
    public class LogService : ILogService
    {
        private readonly IRepository<Log> _logRepository;

        public LogService(IRepository<Log> logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<Log> FindAsync(int id)
        {
            return await _logRepository.FindAsync(id);
        }

        public async Task<IEnumerable<Log>> FindAsync(int[] ids)
        {
            return await _logRepository.Query
                .Filter(l => ids.Contains(l.Id))
                .GetListAsync();
        }

        public async Task<IEnumerable<Log>> GetListAsync(DateTime fromData, DateTime toDate, string level, int page, int size)
        {
            return await _logRepository.Query
                .Filter(l => (l.Logged.Date >= fromData.Date && l.Logged.Date <= toDate.Date) ||
                    string.Equals(l.Level, level, StringComparison.OrdinalIgnoreCase))
                .Sort(o => o.OrderByDescending(l => l.Id))
                .GetListAsync(page, size);
        }

        public async Task<int> DeleteAsync(Log entity)
        {
            return await _logRepository.DeleteCommitAsync(entity);
        }

        public async Task<int> DeleteAsync(IEnumerable<Log> entities)
        {
            foreach (var entity in entities)
            {
                _logRepository.Delete(entity);
            }

            return await _logRepository.CommitAsync();
        }
    }
}
