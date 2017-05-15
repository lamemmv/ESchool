using System;
using System.Threading.Tasks;
using ESchool.Domain.Entities.Messages;
using MailKit.Net.Smtp;
using MimeKit;

namespace ESchool.Services.Messages
{
    public class MailKitEmailSender : IEmailSender
    {
        public async Task SendEmailAsync(
            EmailAccount emailAccount,
            string from,
            string to,
            string subject,
            string body,
            bool htmlBody = true,
            string fromAlias = null,
            string toAlias = null,
            string replyTo = null)
        {
            ValidateEmail(from, to, subject);

            MimeMessage emailMessage = BuildMimeMessage(from, fromAlias, to, toAlias, subject, body, htmlBody, replyTo);

            await SendEmailAsync(emailMessage, emailAccount);
        }

        //public void SendEmailAsyncViaSMTPPickupFolder(string email, string subject, string message)
        //{
        //	var emailMessage = new MimeMessage();

        //	emailMessage.From.Add(new MailboxAddress("Joe Bloggs", "jbloggs@example.com"));
        //	emailMessage.To.Add(new MailboxAddress("", email));
        //	emailMessage.Subject = subject;
        //	emailMessage.Body = new TextPart("plain") { Text = message };

        //	using (StreamWriter data = System.IO.File.CreateText("c:\\smtppickup\\email.txt"))
        //	{
        //		emailMessage.WriteTo(data.BaseStream);
        //	}
        //}

        private void ValidateEmail(string from, string to, string subject)
        {
            if (string.IsNullOrEmpty(from))
            {
                throw new ArgumentException("Email: No From address provided.");
            }

            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentException("Email: No To address provided.");
            }

            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentException("Email: No Subject provided.");
            }
        }

        private MimeMessage BuildMimeMessage(
            string from,
            string fromAlias,
            string toCsv,
            string toAlias,
            string subject,
            string body,
            bool htmlBody,
            string replyTo)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(fromAlias ?? string.Empty, from));

            string[] addresses = toCsv.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string address in addresses)
            {
                emailMessage.To.Add(new MailboxAddress(toAlias ?? string.Empty, address));
            }

            if (!string.IsNullOrEmpty(replyTo))
            {
                emailMessage.ReplyTo.Add(new MailboxAddress(string.Empty, replyTo));
            }

            emailMessage.Subject = subject;

            if (addresses.Length > 0)
            {
                // Send multiple emails.
                emailMessage.Importance = MessageImportance.High;
            }

            BodyBuilder bodyBuilder = new BodyBuilder();

            if (htmlBody)
            {
                bodyBuilder.HtmlBody = body;
            }
            else
            {
                bodyBuilder.TextBody = body;
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private async Task SendEmailAsync(MimeMessage emailMessage, EmailAccount emailAccount)
        {
            using (var client = new SmtpClient())
            {
                client.Connect(emailAccount.Host, emailAccount.Port, emailAccount.EnableSsl);

                // Note: since we don't have an OAuth2 token, disable the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication.
                if (emailAccount.UseDefaultCredentials)
                {
                    await client.AuthenticateAsync(emailAccount.UserName, emailAccount.Password);
                }

                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
