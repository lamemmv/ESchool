using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ESchool.Domain.Entities.Accounts
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

        public bool IsAdmin { get; set; }

        public string DataEventRecordsRole { get; set; }

        public string SecuredFilesRole { get; set; }

        public DateTime AccountExpires { get; set; }
    }
}
