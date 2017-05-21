﻿using System;
using System.Threading.Tasks;
using ESchool.Data.Entities.Messages;
using ESchool.Services.Infrastructure.Tasks;
using Microsoft.Extensions.Logging;

namespace ESchool.Services.Messages
{
    public class QueuedEmailSendTask : BaseBackgroundTask, IBackgroundTask
    {
        private readonly ILogger<QueuedEmailSendTask> _logger;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IEmailSender _emailSender;

        public QueuedEmailSendTask(int taskId, int loopInSeconds)
            : base(taskId, loopInSeconds)
        {
        }

        public QueuedEmailSendTask(
            int taskId,
            int loopInSeconds,
            ILogger<QueuedEmailSendTask> logger,
            IQueuedEmailService queuedEmailService,
            IEmailSender emailSender)
            : base(taskId, loopInSeconds)
        {
            _logger = logger;
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
                size: 512);

            foreach (var queuedEmail in pagedQueuedEmail.Data)
            {
                await SendEmailAsync(queuedEmail);
                await UpdateSentTriesAsync(queuedEmail);
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
                    queuedEmail.ReplyTo,
                    queuedEmail.CC,
                    queuedEmail.BCC);

                queuedEmail.SentOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                queuedEmail.FailedReason = ex.ToString();
            }
        }

        private async Task UpdateSentTriesAsync(QueuedEmail queuedEmail)
        {
            try
            {
                queuedEmail.SentTries = queuedEmail.SentTries + 1;

                await _queuedEmailService.UpdateAsync(queuedEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(0), $"[{nameof(QueuedEmailSendTask)} » {nameof(UpdateSentTriesAsync)}]: {ex.Message}", ex);
            }
        }
    }
}
