using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Data.Paginations;
using ESchool.Domain.Entities.Messages;

namespace ESchool.Services.Messages
{
    public interface IQueuedEmailService
    {
        Task<QueuedEmail> GetAsync(int id);

        Task<IPagedList<QueuedEmail>> GetListAsync(
            DateTime? createdFromUtc,
            DateTime? createdToUtc,
            bool loadNotSentItemsOnly,
            bool loadOnlyItemsToBeSent,
            int maxSendTries,
            bool loadNewest,
            int page,
            int size);

        Task<QueuedEmail> CreateAsync(QueuedEmail entity);

        Task<IList<QueuedEmail>> CreateAsync(IList<QueuedEmail> entities);

        Task<int> UpdateAsync(QueuedEmail entity);

        Task<int> DeleteAsync(int id);

        Task<int> DeleteAsync(QueuedEmail entity);

        Task<int> DeleteAsync(IList<QueuedEmail> entities);

        Task<int> DeleteAll();
    }
}
