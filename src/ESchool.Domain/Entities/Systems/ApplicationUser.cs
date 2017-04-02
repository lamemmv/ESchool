using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ESchool.Domain.Entities.Systems
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
