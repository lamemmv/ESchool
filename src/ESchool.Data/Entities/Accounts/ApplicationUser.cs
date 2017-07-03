using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ESchool.Data.Entities.Accounts
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
            : base()
        {
        }

        public ApplicationUser(string userName)
            : base(userName)
        {
        }
    }
}
