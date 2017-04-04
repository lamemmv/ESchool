using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.Entities.Examinations;

namespace ESchool.Services.Examinations
{
    public interface IQTagService
    {
        Task<QTag> FindAsync(int id);

        Task<QTag> FindAsync(string name);

        Task<IEnumerable<QTag>> GetListAsync();

        Task<int> CreateAsync(QTag entity);

        Task<int> UpdateAsync(QTag entity);

        Task<int> DeleteAsync(QTag entity);
    }
}
