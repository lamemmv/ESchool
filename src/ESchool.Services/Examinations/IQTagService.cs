using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;

namespace ESchool.Services.Examinations
{
    public interface IQTagService : IService
    {
        Task<QTagDto> GetAsync(int id, bool includeParents = false);

        Task<IList<QTagDto>> GetListAsync(int groupId);

        Task<QTag> CreateAsync(QTag entity);

        Task<int> UpdateAsync(QTag entity);

        Task<int> DeleteAsync(int id);
    }
}
