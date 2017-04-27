using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Domain.Enums;

namespace ESchool.Services
{
    public abstract class BaseService : IService
    {
        protected readonly ObjectDbContext _dbContext;

        public BaseService(ObjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorCode> CommitAsync()
        {
            int effectedRows = await _dbContext.SaveChangesAsync();

            return ErrorCode.Success;
        }
    }
}
