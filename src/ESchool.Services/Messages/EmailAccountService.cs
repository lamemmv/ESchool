using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.Entities.Messages;
using ESchool.Services.Constants;
using ESchool.Services.Enums;
using ESchool.Services.Infrastructure.Cache;
using ESchool.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Services.Messages
{
    public class EmailAccountService : BaseService, IEmailAccountService
    {
        private readonly IMemoryCacheService _memoryCacheService;

        public EmailAccountService(
            ObjectDbContext dbContext,
            IMemoryCacheService memoryCacheService)
            : base(dbContext)
        {
            _memoryCacheService = memoryCacheService;
        }

        public async Task<EmailAccount> GetAsync(int id)
        {
            return await EmailAccounts.FindAsync(id);
        }

        public async Task<IList<EmailAccount>> GetListAsync()
        {
            return await EmailAccounts.AsNoTracking()
                .ToListAsync();
        }

        public async Task<EmailAccount> GetDefaultAsync()
        {
            return await _memoryCacheService.GetSlidingExpiration(
                MemoryCacheKeys.DefaultEmailAccountKey,
                () =>
                {
                    return EmailAccounts.AsNoTracking()
                        .FirstOrDefaultAsync(ea => ea.IsDefaultEmailAccount);
                });
        }

        public async Task<EmailAccount> CreateAsync(EmailAccount entity)
        {
            await EmailAccounts.AddAsync(entity);
            await CommitAsync();

            return entity;
        }

        public async Task<int> UpdateAsync(EmailAccount entity)
        {
            EmailAccount updatedEntity = await EmailAccounts.FindAsync(entity.Id);

            if (updatedEntity == null)
            {
                throw new ApiException(
                    $"{nameof(EmailAccount)} not found. Id = {entity.Id}",
                    ApiErrorCode.NotFound);
            }

            updatedEntity.Email = entity.Email;
            updatedEntity.DisplayName = entity.DisplayName;
            updatedEntity.Host = entity.Host;
            updatedEntity.Port = entity.Port;
            updatedEntity.UserName = entity.UserName;
            updatedEntity.Password = entity.Password;
            updatedEntity.EnableSsl = entity.EnableSsl;
            updatedEntity.UseDefaultCredentials = entity.UseDefaultCredentials;
            updatedEntity.IsDefaultEmailAccount = entity.IsDefaultEmailAccount;

            return await CommitAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            EmailAccount entity = await EmailAccounts.FindAsync(id);

            if (entity == null)
            {
                throw new ApiException(
                    $"{nameof(EmailAccount)} not found. Id = {id}",
                    ApiErrorCode.NotFound);
            }

            EmailAccounts.Remove(entity);

            return await CommitAsync();
        }

        private DbSet<EmailAccount> EmailAccounts
        {
            get
            {
                return _dbContext.Set<EmailAccount>();
            }
        }
    }
}
