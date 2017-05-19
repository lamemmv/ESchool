using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Data.Entities.Examinations;
using ESchool.Services.Examinations;
using Microsoft.AspNetCore.Mvc;

namespace ESchool.Admin.Controllers
{
    public class GroupsController : AdminController
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IList<Group>> Get()
        {
            return await _groupService.GetListAsync();
        }
    }
}
