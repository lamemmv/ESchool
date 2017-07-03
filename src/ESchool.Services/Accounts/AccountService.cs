using ESchool.Data;
using ESchool.Services.Enums;

namespace ESchool.Services.Accounts
{
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(ObjectDbContext dbContext)
            : base(dbContext)
        {
        }

        public Permissions ManageAccounts(string value)
        {
            int permission;

            if (int.TryParse(value, out permission))
            {
                return (Permissions)permission;
            }

            return Permissions.None;
        }
    }
}
