using System;
using System.Threading.Tasks;
using ESchool.Domain.Entities.Messages;
using ESchool.Services.Infrastructure.Tasks;

namespace ESchool.Services.Messages
{
    public class QueuedEmailSendTask : BaseBackgroundTask, IBackgroundTask
    {
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IEmailSender _emailSender;

        public QueuedEmailSendTask(int taskId, int loopInSeconds)
            : base(taskId, loopInSeconds)
        {
        }

        public QueuedEmailSendTask(
            int taskId, 
            int loopInSeconds, 
            IQueuedEmailService queuedEmailService,
            IEmailSender emailSender)
            : base(taskId, loopInSeconds)
        {
            _queuedEmailService = queuedEmailService;
            _emailSender = emailSender;
        }

        public override async Task Execute()
        {
            var pagedQueuedEmail = await _queuedEmailService.GetListAsync(
                createdFromUtc: null,
                createdToUtc: null,
                loadNotSentItemsOnly: true,
                loadOnlyItemsToBeSent: true,
                maxSendTries: 3,
                loadNewest: false,
                page: 1,
                size: 500);

            foreach (var queuedEmail in pagedQueuedEmail.Data)
            {
                await SendEmailAsync(queuedEmail);

                queuedEmail.SentTries = queuedEmail.SentTries + 1;

                await _queuedEmailService.UpdateAsync(queuedEmail);
            }
        }

        private async Task SendEmailAsync(QueuedEmail queuedEmail)
        {
            try
            {
                var emailAccount = queuedEmail.EmailAccount;

                await _emailSender.SendEmailAsync(
                    emailAccount,
                    queuedEmail.From,
                    queuedEmail.To,
                    queuedEmail.Subject,
                    queuedEmail.Body,
                    true,
                    queuedEmail.FromName,
                    queuedEmail.ToName,
                    queuedEmail.ReplyTo);

                queuedEmail.SentOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
