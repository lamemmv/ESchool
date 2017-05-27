using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.Entities.Messages;
using ESchool.Data.Enums;
using ESchool.Data.Paginations;
using ESchool.Services.Exceptions;
using ESchool.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Services.Messages
{
    public class QueuedEmailService : BaseService, IQueuedEmailService
    {
        public QueuedEmailService(ObjectDbContext dbContext)
            : base(dbContext)
        {
        }

        //public async Task<QueuedEmail> GetAsync(int id)
        //{
        //    return await QueuedEmails.FindAsync(id);
        //}

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
                DateTime nowUtc = DateTime.UtcNow;
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

        //public async Task<IList<QueuedEmail>> CreateAsync(IList<QueuedEmail> entities)
        //{
        //    await QueuedEmails.AddRangeAsync(entities);
        //    await CommitAsync();

        //    return entities;
        //}

        public async Task<QueuedEmail> CreateAsync(string email, string subject, string message, int emailAccountId = 0)
        {
            EmailAccount emailAccount = emailAccountId == 0 ?
                await EmailAccounts.AsNoTracking()
                    .SingleOrDefaultAsync(ea => ea.Id == emailAccountId && ea.IsDefaultEmailAccount) :
                await EmailAccounts.FindAsync(emailAccountId);

            if (emailAccount == null)
            {
                throw new Exception($"[{nameof(QueuedEmailService)} » {nameof(CreateAsync)}] Default Email Account is null.");
            }

            QueuedEmail queuedEmail = new QueuedEmail
            {
                From = emailAccount.Email,
                FromName = emailAccount.DisplayName,
                To = email,
                Subject = subject,
                Body = message,
                CreatedOnUtc = DateTime.UtcNow,
                Priority = (int)QueuedEmailPriority.High,
                EmailAccountId = emailAccount.Id
            };

            return await CreateAsync(queuedEmail);
        }

        //public async Task<int> UpdateAsync(QueuedEmail entity)
        //{
        //    QueuedEmail updatedEntity = await QueuedEmails.FindAsync(entity.Id);

        //    if (updatedEntity == null)
        //    {
        //        throw new EntityNotFoundException(entity.Id, nameof(QueuedEmail));
        //    }

        //    updatedEntity.EmailAccountId = entity.EmailAccountId;
        //    updatedEntity.Priority = entity.Priority;
        //    updatedEntity.From = entity.From;
        //    updatedEntity.FromName = entity.FromName;
        //    updatedEntity.To = entity.To;
        //    updatedEntity.ToName = entity.ToName;
        //    updatedEntity.ReplyTo = entity.ReplyTo;
        //    updatedEntity.ReplyToName = entity.ReplyToName;
        //    updatedEntity.CC = entity.CC;
        //    updatedEntity.BCC = entity.BCC;
        //    updatedEntity.Subject = entity.Subject;
        //    updatedEntity.Body = entity.Body;
        //    updatedEntity.DontSendBeforeDateUtc = entity.DontSendBeforeDateUtc;

        //    return await CommitAsync();
        //}

        public async Task<int> UpdateAsync(int id, int sentTries, DateTime? sentOnUtc, string failedReason)
        {
            QueuedEmail updatedEntity = await QueuedEmails.FindAsync(id);

            if (updatedEntity == null)
            {
                throw new EntityNotFoundException(id, nameof(QueuedEmail));
            }

            updatedEntity.SentTries = sentTries;
            updatedEntity.SentOnUtc = sentOnUtc;
            updatedEntity.FailedReason = failedReason;

            return await CommitAsync();
        }

        //public async Task<int> DeleteAsync(int id)
        //{
        //    QueuedEmail entity = await QueuedEmails.FindAsync(id);

        //    if (entity == null)
        //    {
        //        throw new EntityNotFoundException(id, nameof(QueuedEmail));
        //    }

        //    QueuedEmails.Remove(entity);

        //    return await CommitAsync();
        //}

        //public async Task<int> DeleteAsync(QueuedEmail entity)
        //{
        //    QueuedEmails.Remove(entity);

        //    return await CommitAsync();
        //}

        //public async Task<int> DeleteAsync(IList<QueuedEmail> entities)
        //{
        //    QueuedEmails.RemoveRange(entities);

        //    return await CommitAsync();
        //}

        //public async Task<int> DeleteAll()
        //{
        //    var queuedEmails = QueuedEmails;

        //    QueuedEmails.RemoveRange(queuedEmails);

        //    return await CommitAsync();
        //}

        private DbSet<QueuedEmail> QueuedEmails
        {
            get
            {
                return _dbContext.Set<QueuedEmail>();
            }
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
