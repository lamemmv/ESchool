using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.Entities.Systems;
using ESchool.Domain.Enums;

namespace ESchool.Services.Systems
{
    public interface ILogService
    {
		Task<Log> FindAsync(int id);

        Task<IEnumerable<Log>> GetListAsync(DateTime fromData, DateTime toDate, string level, int page, int size);

        Task<ErrorCode> DeleteAsync(int id);

		Task<ErrorCode> DeleteAsync(int[] ids);
	}
}
