﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data.Repositories;
using ESchool.Domain.Entities.Systems;
using ESchool.Domain.Enums;

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

        public async Task<IEnumerable<Log>> GetListAsync(DateTime fromData, DateTime toDate, string level, int page, int size)
        {
            return await _logRepository.QueryNoTracking
                .Filter(l => (l.Logged.Date >= fromData.Date && l.Logged.Date <= toDate.Date) ||
                    string.Equals(l.Level, level, StringComparison.OrdinalIgnoreCase))
                .Sort(o => o.OrderByDescending(l => l.Id))
                .GetListAsync(page, size);
        }

        public async Task<ErrorCode> DeleteAsync(int id)
        {
            var entity = await FindAsync(id);

            if (entity == null)
            {
                return ErrorCode.NotFound;
            }

            await _logRepository.DeleteCommitAsync(entity);

            return ErrorCode.Success;
        }

        public async Task<ErrorCode> DeleteAsync(int[] ids)
        {
            await _logRepository.DeleteCommitAsync(l => ids.Contains(l.Id));

            return ErrorCode.Success;
        }
    }
}
