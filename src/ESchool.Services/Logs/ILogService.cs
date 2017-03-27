using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Data.Paginations;
using ESchool.DomainModels.Entities.Logs;

namespace ESchool.Services.Logs
{
	public interface ILogService
    {
		Task<Log> GetSingleAsync(int id);

		Task<IPagedList<Log>> GetLogsAsync(DateTime fromData, DateTime toDate, string level, int page, int size);

		Task<int> DeleteAsync(Log entity);

		Task<int> DeleteAsync(IEnumerable<int> ids);
	}
}
