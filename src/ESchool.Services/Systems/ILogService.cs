using System;
using System.Threading.Tasks;
using ESchool.Data.Paginations;
using ESchool.Domain.Entities.Systems;
using ESchool.Domain.Enums;

namespace ESchool.Services.Systems
{
    public interface ILogService : IService
    {
        Task<Log> FindAsync(int id);

        Task<IPagedList<Log>> GetListAsync(DateTime fromData, DateTime toDate, string level, int page, int size);

        Task<ErrorCode> DeleteAsync(int id);

        Task<ErrorCode> DeleteAsync(int[] ids);
	}
}
