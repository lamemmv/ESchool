using System.Threading.Tasks;
using ESchool.Domain.Entities.Messages;

namespace ESchool.Services.Messages
{
    public interface IEmailSender
    {
        Task SendEmailAsync(
            EmailAccount emailAccount,
            string from,
            string toCsv,
            string subject,
            string body,
            bool htmlBody = true,
            string fromAlias = null,
            string toAlias = null,
            string replyTo = null,
            string replyToAlias = null,
            string ccCsv = null,
            string bccCsv = null);
    }
}
