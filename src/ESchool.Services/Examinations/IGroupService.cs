using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.DTOs.Examinations;

namespace ESchool.Services.Examinations
{
    public interface IGroupService
    {
        Task<IList<GroupDto>> GetListAsync();
    }
}
