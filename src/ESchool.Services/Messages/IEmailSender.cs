using System.Threading.Tasks;
using ESchool.Domain.Entities.Messages;

namespace ESchool.Services.Messages
{
    public interface IEmailSender
    {
        Task SendEmailAsync(
            EmailAccount emailAccount,
            string from,
            string to,
            string subject,
            string body,
            bool htmlBody = true,
            string fromAlias = null,
            string toAlias = null,
            string replyTo = null);
    }
}
