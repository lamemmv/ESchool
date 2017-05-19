using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.Entities.Examinations;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Services.Examinations
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(ObjectDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<IList<Group>> GetListAsync()
        {
            return await Groups.AsNoTracking()
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
