using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Domain.DTOs.Examinations;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Services.Examinations
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(ObjectDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<IList<GroupDto>> GetListAsync()
        {
            return await Groups.AsNoTracking()
                .Select(g => g.ToGroupDto())
                .ToListAsync();
        }

        private DbSet<Group> Groups
        {
            get
            {
                return _dbContext.Set<Group>();
            }
        }
    }
}
