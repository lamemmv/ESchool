using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.Entities.Systems;

namespace ESchool.Services.Systems
{
    public interface ILogService
    {
		Task<Log> FindAsync(int id);

        Task<IEnumerable<Log>> FindAsync(int[] ids);

        Task<IEnumerable<Log>> GetListAsync(DateTime fromData, DateTime toDate, string level, int page, int size);

        Task<int> DeleteAsync(Log entity);

		Task<int> DeleteAsync(IEnumerable<Log> entities);
	}
}
