using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Domain.Entities.Systems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ESchool.Data
{
    public sealed class DbInitializer
    {
        public void Initialize(
            ObjectDbContext dbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            dbContext.Database.EnsureCreated();

            var seedIdentityTask = SeedIdentity(roleManager, userManager);
            seedIdentityTask.Wait();

            //SeedScheduleTasks(dbContext);
        }

        public IDictionary<string, string> SeedSettings(ObjectDbContext dbContext)
        {
            var configSettings = new Dictionary<string, string>
            {
                { "account.lockoutonfailure", "true" }
            };

            dbContext.Set<Setting>().AddRange(
                configSettings.Select(kvp => new Setting { Name = kvp.Key, Value = kvp.Value }));

            dbContext.SaveChanges();

            return configSettings;
        }

        private async Task<IdentityResult> SeedIdentity(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            var roles = new string[] { "Administrators", "Supervisors", "Moderators", "Registereds", "Guests" };

            foreach (var roleName in roles)
            {
				var identityRole = await roleManager.FindByNameAsync(roleName).ConfigureAwait(false);

				if (identityRole == null)
				{
					await roleManager.CreateAsync(new IdentityRole(roleName)).ConfigureAwait(false);
				}
            }

            string userName = "sa";
            string email = "ankn85@yahoo.com";
            string password = "1qazXSW@";

            var user = await userManager.FindByNameAsync(email).ConfigureAwait(false);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userName,
                    Email = email,
                    EmailConfirmed = true,
                    LockoutEnabled = true
                };

                var result = await userManager.CreateAsync(user, password).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    user = await userManager.FindByNameAsync(email).ConfigureAwait(false);

                    await userManager.AddToRolesAsync(user, roles).ConfigureAwait(false);
                }
            }

            return await Task.FromResult(new IdentityResult());
        }
    }
}
