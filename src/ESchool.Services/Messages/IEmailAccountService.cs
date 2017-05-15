using System.Collections.Generic;
using System.Threading.Tasks;
using ESchool.Domain.Entities.Messages;

namespace ESchool.Services.Messages
{
    public interface IEmailAccountService
    {
        Task<EmailAccount> GetAsync(int id);

        Task<IList<EmailAccount>> GetListAsync();

        Task<EmailAccount> CreateAsync(EmailAccount entity);

        Task<int> UpdateAsync(EmailAccount entity);

        Task<int> DeleteAsync(int id);
    }
}
