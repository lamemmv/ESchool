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

            // This protects from deadlocks by starting the async method on the ThreadPool.
            var resultIdentity = Task.Run(() => SeedIdentity(roleManager, userManager)).Result;

            //SeedScheduleTasks(dbContext);

            var resultDataTest = Task.Run(() => new DbTestDataInitializer(dbContext).SeedAsync()).Result;
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
                var identityRole = await roleManager.FindByNameAsync(roleName);

				if (identityRole == null)
				{
                    await roleManager.CreateAsync(new IdentityRole(roleName));
				}
            }

            string userName = "sa";
            string email = "ankn85@yahoo.com";
            string password = "1qazXSW@";

            var user = await userManager.FindByNameAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userName,
                    Email = email,
                    EmailConfirmed = true,
                    LockoutEnabled = true
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    user = await userManager.FindByNameAsync(email);

                    await userManager.AddToRolesAsync(user, roles);
                }
            }

            return await Task.FromResult(new IdentityResult());
        }
    }
}
