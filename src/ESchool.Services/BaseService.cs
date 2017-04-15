using System;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace ESchool.Services
{
    public abstract class BaseService : IService
    {
        protected readonly ObjectDbContext _dbContext;
        protected readonly ILogger<BaseService> _logger;

        public BaseService(ObjectDbContext dbContext, ILogger<BaseService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ErrorCode> CommitAsync()
        {
            try
            {
                int effectedRows = await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0), ex, ex.Message);

                return ErrorCode.InternalServerError;
            }

            return ErrorCode.Success;
        }
    }
}
