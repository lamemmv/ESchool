using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.Entities.Examinations;

namespace ESchool.Services.Examinations
{
    public interface IQTagService : IService<QTag>
    {
        Task<IEnumerable<QTag>> GetListAsync();
    }
}
