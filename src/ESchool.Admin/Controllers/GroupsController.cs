using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Services.Examinations;

namespace ESchool.Admin.Controllers
{
    public class GroupsController : AdminController
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        public async Task<IList<GroupDto>> GetListAsync()
        {
            return await _groupService.GetListAsync();
        }
    }
}
