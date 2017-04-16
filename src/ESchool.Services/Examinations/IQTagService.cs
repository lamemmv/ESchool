using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Enums;

namespace ESchool.Services.Examinations
{
    public interface IQTagService : IService
    {
        Task<QTag> FindAsync(int id);

        Task<QTagDto> GetAsync(int id);

        Task<IEnumerable<QTagDto>> GetListAsync();

        Task<ErrorCode> CreateAsync(QTag entity);

        Task<ErrorCode> UpdateAsync(QTag entity);

        Task<ErrorCode> DeleteAsync(int id);
    }
}
