using System.Threading.Tasks;
using ESchool.Data;

namespace ESchool.Services
{
    public abstract class BaseService : IService
    {
        protected readonly ObjectDbContext _dbContext;

        public BaseService(ObjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
