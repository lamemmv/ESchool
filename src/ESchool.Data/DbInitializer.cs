using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Entities.Systems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
            Task.Run(() => SeedIdentity(roleManager, userManager)).Wait();

            Task.Run(() => SeedGroupsAsync(dbContext)).Wait();
            Task.Run(() => SeedQTagsAsync(dbContext)).Wait();

            //SeedScheduleTasks(dbContext);

            //var resultDataTest = Task.Run(() => new DbTestDataInitializer(dbContext).SeedAsync()).Result;
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

        private async Task<int> SeedGroupsAsync(ObjectDbContext dbContext)
        {
            var groups = new List<string> { "Khối 6", "Khối 7", "Khối 8", "Khối 9" };

            var qtagDbSet = dbContext.Set<Group>();
            var existingGroups = qtagDbSet.AsNoTracking().Where(g => groups.Contains(g.Name));

            if (await existingGroups.AnyAsync())
            {
                groups = groups.Except(existingGroups.Select(t => t.Name)).ToList();
            }

            await qtagDbSet.AddRangeAsync(groups.Select(g => new Group { Name = g }));

            return await dbContext.SaveChangesAsync();
        }

        private async Task<int> SeedQTagsAsync(ObjectDbContext dbContext)
        {
            var qtags = new List<string> { "Kỹ năng tính toán cơ bản", "Nâng cao", "Kiến thức cũ" };
            var qtagDbSet = dbContext.Set<QTag>();

            var groupDbSet = dbContext.Set<Group>();
            var groups = await groupDbSet
                .Include(g => g.QTags)
                .ToListAsync();

            bool needToSaveChange = false;

            foreach (var group in groups)
            {
                var existingQTags = group.QTags.Where(t => qtags.Contains(t.Name));

                if (existingQTags.Any())
                {
                    qtags = qtags.Except(existingQTags.Select(t => t.Name)).ToList();
                }

                foreach (var qtag in qtags)
                {
                    group.QTags.Add(new QTag { ParentId = 0, Name = qtag, Description = qtag, Group = new Group { Id = group.Id } });
                    needToSaveChange = true;
                }
            }

            if (needToSaveChange)
            {
                await dbContext.SaveChangesAsync();
            }

            return 0;
        }
    }
}
