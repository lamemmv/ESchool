using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.Paginations;
using ESchool.Domain.Entities.Messages;
using ESchool.Services.Exceptions;
using ESchool.Services.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Services.Messages
{
    public class QueuedEmailService : BaseService, IQueuedEmailService
    {
        public QueuedEmailService(ObjectDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<QueuedEmail> GetAsync(int id)
        {
            return await QueuedEmails.FindAsync(id);
        }

        public async Task<IPagedList<QueuedEmail>> GetListAsync(
            DateTime? createdFromUtc,
            DateTime? createdToUtc,
            bool loadNotSentItemsOnly,
            bool loadOnlyItemsToBeSent,
            int maxSendTries,
            bool loadNewest,
            int page,
            int size)
        {
            var query = QueuedEmails
                .Include(qe => qe.EmailAccount)
                .AsNoTracking();

            if (createdFromUtc.HasValue)
            {
                var startDate = createdFromUtc.Value.StartOfDay();
                query = query.Where(qe => qe.CreatedOnUtc >= startDate);
            }

            if (createdToUtc.HasValue)
            {
                var endDate = createdToUtc.Value.EndOfDay();
                query = query.Where(qe => qe.CreatedOnUtc <= endDate);
            }

            if (loadNotSentItemsOnly)
            {
                query = query.Where(qe => !qe.SentOnUtc.HasValue);
            }

            if (loadOnlyItemsToBeSent)
            {
                var nowUtc = DateTime.UtcNow;
                query = query.Where(qe => !qe.DontSendBeforeDateUtc.HasValue || qe.DontSendBeforeDateUtc.Value <= nowUtc);
            }

            query = query.Where(qe => qe.SentTries < maxSendTries);

            query = loadNewest ?
                query.OrderByDescending(qe => qe.CreatedOnUtc) :
                query.OrderByDescending(qe => qe.Priority).ThenBy(qe => qe.CreatedOnUtc);

            return await query.GetListAsync(page, size);
        }

        public async Task<QueuedEmail> CreateAsync(QueuedEmail entity)
        {
            await QueuedEmails.AddAsync(entity);
            await CommitAsync();

            return entity;
        }

        public async Task<IList<QueuedEmail>> CreateAsync(IList<QueuedEmail> entities)
        {
            await QueuedEmails.AddRangeAsync(entities);
            await CommitAsync();

            return entities;
        }

        public async Task<int> UpdateAsync(QueuedEmail entity)
        {
            var updatedEntity = await QueuedEmails.FindAsync(entity.Id);

            if (updatedEntity == null)
            {
                throw new EntityNotFoundException("Queued Email not found.");
            }

            updatedEntity.EmailAccountId = entity.EmailAccountId;
            updatedEntity.Priority = entity.Priority;
            updatedEntity.From = entity.From;
            updatedEntity.FromName = entity.FromName;
            updatedEntity.To = entity.To;
            updatedEntity.ToName = entity.ToName;
            updatedEntity.ReplyTo = entity.ReplyTo;
            updatedEntity.ReplyToName = entity.ReplyToName;
            updatedEntity.CC = entity.CC;
            updatedEntity.BCC = entity.BCC;
            updatedEntity.Subject = entity.Subject;
            updatedEntity.Body = entity.Body;
            updatedEntity.DontSendBeforeDateUtc = entity.DontSendBeforeDateUtc;

            return await CommitAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await QueuedEmails.FindAsync(id);

            if (entity == null)
            {
                throw new EntityNotFoundException("Queued Emails not found.");
            }

            QueuedEmails.Remove(entity);

            return await CommitAsync();
        }

        public async Task<int> DeleteAsync(QueuedEmail entity)
        {
            QueuedEmails.Remove(entity);

            return await CommitAsync();
        }

        public async Task<int> DeleteAsync(IList<QueuedEmail> entities)
        {
            QueuedEmails.RemoveRange(entities);

            return await CommitAsync();
        }

        public async Task<int> DeleteAll()
        {
            var queuedEmails = QueuedEmails;

            QueuedEmails.RemoveRange(queuedEmails);

            return await CommitAsync();
        }

        private DbSet<QueuedEmail> QueuedEmails
        {
            get
            {
                return _dbContext.Set<QueuedEmail>();
            }
        }
    }
}
