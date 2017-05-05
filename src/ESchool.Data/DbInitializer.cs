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
            var groupDbSet = dbContext.Set<Group>();
            var qtagDbSet = dbContext.Set<QTag>();

            await SeedGroup6QTagsAsync(dbContext, groupDbSet, qtagDbSet);
            await SeedGroup7QTagsAsync(dbContext, groupDbSet, qtagDbSet);
            await SeedGroup8QTagsAsync(dbContext, groupDbSet, qtagDbSet);
            await SeedGroup9QTagsAsync(dbContext, groupDbSet, qtagDbSet);
        }

        private async Task SeedGroup6QTagsAsync(ObjectDbContext dbContext, DbSet<Group> groupDbSet, DbSet<QTag> qtagDbSet)
        {
            var groupName = "Khối 6";
            var rootQTagNames = new List<string> { "Kỹ năng tính toán cơ bản", "Kiến thức cũ", "Kiến thức hiện tại", "Kiến thức trước chương trình", "Nâng cao" };
            var level1QTagNames = new List<string> { "Đại số", "Hình học" };
            var subDaiSoQTagNames = new List<string>
            {
                "Tập hợp",
                "Các phép toán trên tập hợp số tự nhiên",
                "So sánh hai lũy thừa",
                "Tính chia hết trên tập hợp số tự nhiên",
                "Số nguyên tố. Hợp số",
                "Uớc chung lớn nhất. Bội chung nhỏ nhất",
                "Số nguyên",
                "Phân số",
                "Hỗn số. số thập phân. Phần trăm.",
                "Ba bài toán cơ bản về phân số"
            };
            var subHinhHocQTagNames = new List<string>
            {
                "Đoạn thẳng",
                "Góc"
            };

            await SeedGroupsAndQTagsAsync(dbContext, groupDbSet, qtagDbSet, groupName, rootQTagNames, level1QTagNames, subDaiSoQTagNames, subHinhHocQTagNames);
        }

        private async Task SeedGroup7QTagsAsync(ObjectDbContext dbContext, DbSet<Group> groupDbSet, DbSet<QTag> qtagDbSet)
        {
            var groupName = "Khối 7";
            var rootQTagNames = new List<string> { "Kỹ năng tính toán cơ bản", "Kiến thức cũ", "Kiến thức hiện tại", "Kiến thức trước chương trình", "Nâng cao" };
            var level1QTagNames = new List<string> { "Đại số", "Hình học" };
            var subDaiSoQTagNames = new List<string>
            {
                "Các phép toán trên tập hợp số hữu tỉ",
                "So sánh hai số hữu tỉ",
                "Giá trị tuyệt đối của một số hữu tỉ",
                "Lũy thừa của một số hữu tỉ",
                "Tỉ lệ thức",
                "Số thập phân hữu hạn. Số thập phân vô hạn tuần hoàn. Làm tròn số",
                "Số vô tỉ. Khái niệm căn bậc hai. Số thực",
                "Đại lượng tỉ lệ thuận",
                "Đại lượng tỉ lệ nghịch",
                "Hàm số và đồ thị",
                "Thống kê",
                "Biểu thức đại số",
            };
            var subHinhHocQTagNames = new List<string>
            {
                "Đường thẳng song song. Đường thẳng vuông góc",
                "Tam giác",
                "Quan hệ giữa các yếu tố trong tam giác",
                "Các đường đồng quy trong tam giác"
            };

            await SeedGroupsAndQTagsAsync(dbContext, groupDbSet, qtagDbSet, groupName, rootQTagNames, level1QTagNames, subDaiSoQTagNames, subHinhHocQTagNames);
        }

        private async Task SeedGroup8QTagsAsync(ObjectDbContext dbContext, DbSet<Group> groupDbSet, DbSet<QTag> qtagDbSet)
        {
            var groupName = "Khối 8";
            var rootQTagNames = new List<string> { "Kỹ năng tính toán cơ bản", "Kiến thức cũ", "Kiến thức hiện tại", "Kiến thức trước chương trình", "Nâng cao" };
            var level1QTagNames = new List<string> { "Đại số", "Hình học" };
            var subDaiSoQTagNames = new List<string>
            {
                "Nhân đa thức",
                "Những hằng đẳng thức đáng nhớ",
                "Phân tích đa thức thành nhân tử",
                "Chia đa thức",
                "Phân thức đại số",
                "Phương trình bậc nhất một ẩn",
                "Giải toán bằng cách lập phương trình",
                "Bất phương trình"
            };
            var subHinhHocQTagNames = new List<string>
            {
                "Tứ giác. Dấu hiệu nhận biết các hình",
                "Chủ đề 2 – Đa giác. Diện tích đa giác",
                "Định lí TA-LET",
                "Tính chất đường phân giác của tam giác",
                "Tam giác đồng dạng",
                "Hình lăng trụ đứng",
                "Tình chóp đều"
            };

            await SeedGroupsAndQTagsAsync(dbContext, groupDbSet, qtagDbSet, groupName, rootQTagNames, level1QTagNames, subDaiSoQTagNames, subHinhHocQTagNames);
        }

        private async Task SeedGroup9QTagsAsync(ObjectDbContext dbContext, DbSet<Group> groupDbSet, DbSet<QTag> qtagDbSet)
        {
            var groupName = "Khối 9";
            var rootQTagNames = new List<string> { "Kỹ năng tính toán cơ bản", "Kiến thức cũ", "Kiến thức hiện tại", "Kiến thức trước chương trình", "Nâng cao" };
            var level1QTagNames = new List<string> { "Đại số", "Hình học" };
            var subDaiSoQTagNames = new List<string>
            {
                "Căn bậc hai và các phép biến đổi",
                "Rút gọn biểu thức chứa căn bậc hai",
                "Căn bậc ba",
                "Hàm số bậc nhất",
                "Vị trí tương đối của hai đường thẳng",
                "Hệ số góc của đường thẳng",
                "Một số bài toán về đường thẳng",
                "Hệ phương trình bậc nhất hai ẩn",
                "Giải bài toán bằng cách lập hệ phương trình",
                "Hàm số y = ax2",
                "Phương trình bậc hai một ẩn",
                "Hệ thức VI-ET và ứng dụng",
                "Tương giáo giữa đường thẳng và Parabol",
                "Phương trình quy về phương trình bậc hai một ẩn",
                "Giải bài toán bằng cách lập phương trình"
            };
            var subHinhHocQTagNames = new List<string>
            {
                "Hệ thức lượng trong tam giác vuông",
                "Đường tròn",
                "Góc với đường tròn"
            };

            await SeedGroupsAndQTagsAsync(dbContext, groupDbSet, qtagDbSet, groupName, rootQTagNames, level1QTagNames, subDaiSoQTagNames, subHinhHocQTagNames);
        }

        private async Task SeedGroupsAndQTagsAsync(
            ObjectDbContext dbContext, 
            DbSet<Group> groupDbSet, 
            DbSet<QTag> qtagDbSet,
            string groupName,
            IList<string> rootQTagNames,
            IList<string> level1QTagNames,
            IList<string> subDaiSoQTagNames,
            IList<string> subHinhHocQTagNames)
        {
            var group = await groupDbSet.SingleOrDefaultAsync(g => g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));

            if (group == null)
            {
                var rootQTags = rootQTagNames.Select(t => new QTag
                {
                    ParentId = 0,
                    Name = t,
                    Description = t
                }).ToList();

                group = new Group
                {
                    Name = groupName,
                    QTags = rootQTags
                };

                await groupDbSet.AddAsync(group);
                await dbContext.SaveChangesAsync();

                // Level 1.
                var level1QTags = new List<QTag>();

                foreach (var qtag in rootQTags)
                {
                    level1QTags.AddRange(level1QTagNames.Select(t => new QTag
                    {
                        GroupId = group.Id,
                        ParentId = qtag.Id,
                        Name = t,
                        Description = t
                    }));
                }

                await qtagDbSet.AddRangeAsync(level1QTags);
                await dbContext.SaveChangesAsync();

                // Level 12.
                var daisoQTags = level1QTags.Where(t => t.Name.Equals("Đại số", StringComparison.OrdinalIgnoreCase));

                foreach (var qtag in daisoQTags)
                {
                    await qtagDbSet.AddRangeAsync(subDaiSoQTagNames.Select(t => new QTag
                    {
                        GroupId = group.Id,
                        ParentId = qtag.Id,
                        Name = t,
                        Description = t
                    }));
                }

                var hinhhocQTags = level1QTags.Where(t => t.Name.Equals("Hình học", StringComparison.OrdinalIgnoreCase));

                foreach (var qtag in hinhhocQTags)
                {
                    await qtagDbSet.AddRangeAsync(subHinhHocQTagNames.Select(t => new QTag
                    {
                        GroupId = group.Id,
                        ParentId = qtag.Id,
                        Name = t,
                        Description = t
                    }));
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
