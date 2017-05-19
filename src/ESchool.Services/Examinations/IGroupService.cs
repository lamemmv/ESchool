using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Data.Entities.Examinations;

namespace ESchool.Services.Examinations
{
    public interface IGroupService
    {
        Task<IList<Group>> GetListAsync();
    }
}
