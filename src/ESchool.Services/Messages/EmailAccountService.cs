using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Domain.Entities.Messages;
using ESchool.Services.Exceptions;
using ESchool.Services.Infrastructure;
using ESchool.Services.Infrastructure.Cache;
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
                ApplicationConsts.DefaultEmailAccountKey,
                () =>
                {
                    return EmailAccounts.AsNoTracking()
                        .SingleOrDefaultAsync(ea => ea.IsDefaultEmailAccount);
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
            var updatedEntity = await EmailAccounts.FindAsync(entity.Id);

            if (updatedEntity == null)
            {
                throw new EntityNotFoundException("Email Account not found.");
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
            var entity = await EmailAccounts.FindAsync(id);

            if (entity == null)
            {
                throw new EntityNotFoundException("Email Account not found.");
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
