using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;

namespace ESchool.Services.Examinations
{
    public interface IQTagService : IService
    {
        Task<QTag> FindAsync(int id);

        Task<QTagDto> GetAsync(int id);

        Task<IEnumerable<QTagDto>> GetListAsync();

        Task<QTag> CreateAsync(QTag entity);

        Task<int> UpdateAsync(QTag entity);

        Task<int> DeleteAsync(int id);
    }
}
