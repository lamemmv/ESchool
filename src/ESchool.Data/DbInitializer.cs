using System;
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

            Task.Run(() => SeedGroupsAndQTagsAsync(dbContext)).Wait();

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

        private async Task SeedIdentity(
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
                    await userManager.AddToRolesAsync(user, roles);
                }
            }
        }

        private async Task SeedGroupsAndQTagsAsync(ObjectDbContext dbContext)
        {
            var groupNames = new List<string> { "Khối 6", "Khối 7", "Khối 8", "Khối 9" };
            var qtagNames = new List<string> { "Kỹ năng tính toán cơ bản", "Kiến thức cũ", "Kiến thức hiện tại", "Kiến thức trước chương trình", "Nâng cao" };
            var subQTagNames = new List<string> { "Đại số", "Hình học" };

            var groupDbSet = dbContext.Set<Group>();
            var qtagDbSet = dbContext.Set<QTag>();

            if (!await groupDbSet.AnyAsync() && !await qtagDbSet.AnyAsync())
            {
                // Add Groups.
                var groups = groupNames.Select(g => new Group
                {
                    Name = g,
                    QTags = qtagNames.Select(t => new QTag
                    {
                        ParentId = 0,
                        Name = t,
                        Description = t
                    }).ToList()
                }).ToList();

                await groupDbSet.AddRangeAsync(groups);
                await dbContext.SaveChangesAsync();

                // Add Sub QTags.
                foreach (var qtagName in qtagNames)
                {
                    var qtags = qtagDbSet.AsNoTracking()
                        .Where(t => t.Name.Equals(qtagName, StringComparison.OrdinalIgnoreCase));

                    foreach (var qtag in qtags)
                    {
                        await qtagDbSet.AddRangeAsync(subQTagNames.Select(t => new QTag
                        {
                            ParentId = qtag.Id,
                            GroupId = qtag.GroupId,
                            Name = t,
                            Description = t
                        }));
                    }

                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
