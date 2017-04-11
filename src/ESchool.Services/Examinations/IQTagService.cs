using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Enums;

namespace ESchool.Services.Examinations
{
    public interface IQTagService
    {
        Task<QTag> FindAsync(int id);

        Task<IEnumerable<QTag>> GetListAsync();

        Task<ErrorCode> CreateAsync(QTag entity);

        Task<ErrorCode> UpdateAsync(QTag entity);

        Task<ErrorCode> DeleteAsync(int id);
    }
}
