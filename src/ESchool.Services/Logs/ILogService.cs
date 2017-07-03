using System;
using System.Threading.Tasks;
using ESchool.Data.Entities.Logs;
using ESchool.Data.Paginations;

namespace ESchool.Services.Logs
{
    public interface ILogService : IService
    {
        Task<Log> FindAsync(int id);

        Task<IPagedList<Log>> GetListAsync(DateTime fromDate, DateTime toDate, string level, int page, int size);

        Task<int> DeleteAsync(int id);

        Task<int> DeleteAsync(int[] ids);
	}
}
