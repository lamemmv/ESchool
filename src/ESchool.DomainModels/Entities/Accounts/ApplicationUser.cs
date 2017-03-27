using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ESchool.DomainModels.Entities.Accounts
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
