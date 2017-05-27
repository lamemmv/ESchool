using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.Entities.Accounts;
using ESchool.Data.Entities.Examinations;
using ESchool.Data.Entities.Messages;
using ESchool.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Core;
using OpenIddict.Models;

namespace ESchool.Services.AppStart
{
    public sealed class DbInitializer
    {
        public async Task Initialize(
            ObjectDbContext dbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            OpenIddictApplicationManager<OpenIddictApplication> applicationManager)
        {
            await dbContext.Database.EnsureCreatedAsync();

            await SeedIdentityAsync(roleManager, userManager);

            await SeedOpenIddict(applicationManager);

            await SeedEmailAccountsAsync(dbContext);

            await SeedGroupsAndQTagsAsync(dbContext);

            //SeedScheduleTasks(dbContext);

            //var resultDataTest = Task.Run(() => new DbTestDataInitializer(dbContext).SeedAsync()).Result;
        }

        private async Task SeedIdentityAsync(
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

        private static async Task SeedOpenIddict(OpenIddictApplicationManager<OpenIddictApplication> applicationManager)
        {
            string clientId = "ESchool.Web";
            string clientPassword = clientId.ToLowerInvariant() + ".secret";

            if (await applicationManager.FindByClientIdAsync(clientId, CancellationToken.None) == null)
            {
                var application = new OpenIddictApplication
                {
                    ClientId = clientId,
                    DisplayName = clientId,
                    LogoutRedirectUri = "http://localhost:59999/",
                    RedirectUri = "http://localhost:59999/signin-oidc"
                };

                await applicationManager.CreateAsync(application, clientPassword, CancellationToken.None);
            }
        }

        private async Task SeedEmailAccountsAsync(ObjectDbContext dbContext)
        {
            string email = "eschoolapi@gmail.com";
            string password = "1qaw3(OLP_";

            var emailAccountDbSet = dbContext.Set<EmailAccount>();

            if (!await emailAccountDbSet.AnyAsync(e => e.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                await emailAccountDbSet.AddAsync(new EmailAccount
                {
                    Email = email,
                    DisplayName = "No Reply",
                    Host = "smtp.gmail.com",
                    Port = 587,
                    UserName = email,
                    Password = password,
                    EnableSsl = false,
                    UseDefaultCredentials = true,
                    IsDefaultEmailAccount = true
                });

                await dbContext.SaveChangesAsync();
            }
        }

        #region Groups and QTags

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

            await SeedGroupsAndQTagsAsync(
                dbContext, 
                groupDbSet, 
                qtagDbSet, 
                groupName, 
                rootQTagNames, 
                level1QTagNames, 
                subDaiSoQTagNames, 
                subHinhHocQTagNames,
                GetQuestionsKhoi6DaiSo(),
                GetQuestionsKhoi6HinhHoc());
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
            IList<string> subHinhHocQTagNames,
            IList<Question> daisoQuestions = null,
            IList<Question> hinhhocQuestions = null)
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
                int index = 0;
                int numberOfQuestionPerQTag = daisoQuestions != null ? daisoQuestions.Count / 50 : 0;

                var daisoQTags = level1QTags.Where(t => t.Name.Equals("Đại số", StringComparison.OrdinalIgnoreCase));

                foreach (var qtag in daisoQTags)
                {
                    foreach (var name in subDaiSoQTagNames)
                    {
                        var subQTag = new QTag
                        {
                            GroupId = group.Id,
                            ParentId = qtag.Id,
                            Name = name,
                            Description = name
                        };

                        if (numberOfQuestionPerQTag != 0)
                        {
                            subQTag.Questions = daisoQuestions.Skip(index * numberOfQuestionPerQTag).Take(numberOfQuestionPerQTag).ToList();
                            index++;
                        }

                        await qtagDbSet.AddAsync(subQTag);
                    }
                }

                index = 0;
                numberOfQuestionPerQTag = hinhhocQuestions != null ? hinhhocQuestions.Count / 10 : 0;

                var hinhhocQTags = level1QTags.Where(t => t.Name.Equals("Hình học", StringComparison.OrdinalIgnoreCase));

                foreach (var qtag in hinhhocQTags)
                {
                    foreach (var name in subHinhHocQTagNames)
                    {
                        var subQTag = new QTag
                        {
                            GroupId = group.Id,
                            ParentId = qtag.Id,
                            Name = name,
                            Description = name
                        };

                        if (numberOfQuestionPerQTag != 0)
                        {
                            subQTag.Questions = hinhhocQuestions.Skip(index * numberOfQuestionPerQTag).Take(numberOfQuestionPerQTag).ToList();
                            index++;
                        }

                        await qtagDbSet.AddAsync(subQTag);
                    }
                }

                await dbContext.SaveChangesAsync();
            }
        }

        private List<Question> GetQuestionsKhoi6DaiSo()
        {
            var type = (int)QuestionType.SingleChoice;
            var currentDate = DateTime.UtcNow.Date;

            return new List<Question>
            {
                new Question
                {
                    Content = "Số các số tự nhiên có 4 chữ số là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "8999 số",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "9000 số",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "9800 số",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Một kết quả khác",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Nếu (- 16) . x = - 112 thì giá trị của x là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "7",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "-7",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "116",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "-116",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Chọn câu trả lời đúng:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "- 365 . 366 < 1",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "- 365 . 366 = 1",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "- 365 . 366 = - 1",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "- 365 . 366 > 1",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Cho x thuộc Z và - 7 < x < 8.  Tổng các số nguyên x bằng:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "0",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "-7",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "-6",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "7",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Tìm x, biết IxI + 5 = 4.",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "x = - 1",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "x = - 9",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "x = - 1 hoặc x = - 9",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "x thuộc Ø",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Khi phân tích số 13920 ra thừa số nguyên tố thì số 13920:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Có thừa số nguyên tố 2 và 5.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Có thừa số nguyên tố 3 và 5.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Có thừa số nguyên tố 2; 3; 5; và 13.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Có thừa số nguyên tố 2; 3; 5",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Các số có hai chữ số là bình phương của một số nguyên tố là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "25; 49",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "25; 81; 62",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "49; 74",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "25; 22",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tổng của ba số tự nhiên liên tiếp là một số:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Chia cho 3 dư 1",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Chia cho 3 dư 2",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Chia hết cho 3",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Không chia hết cho 3",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tìm số tự nhiên x, biết: 5x + 3x = 88",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "x = 11",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "x = 5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "x = 8",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Một kết quả khác.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "<p>Cho E = {5; - 8; 0}.</p><p>Tập hợp F bao gồm các phần tử của E và các số đối của chúng là:</p>",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "F = {5; -8; 0; - 5}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "F = {- 5; 8; 0}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "F = {5; - 5; 0; - 8}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "F ={5; - 8; 0; - 5; 8}",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Giá trị của biểu thức: x + y + z với x = - 2843; y = 2842 và z = 19 là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "- 31",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "20",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "19",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "18",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Tập hợp có 3 phần tử là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = " {0; 1}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "{0; a; b}",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "{Bưởi, cam, chanh, táo}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "{6A; 6B}",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Cho tập hợp M = {0; 1; 3; 5}. Kết luận nào sau đây là đúng.",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "0 là tập con của M.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "{1; 0} thuộc M.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "{1; 2; 3} là tập con của M.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "{0} là tập con của M.",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Kết quả của phép tính (-5) + (-6) là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "11",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "-11",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "1",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "-1",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tổng của các số nguyên x mà -5 ≤ x < 6 bằng:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "-5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "0",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "-6",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "BCNN (4; 18) là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "18",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "36",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "54",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "72",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Nếu x + 9 = 5 thì x bằng:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "-3",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "3",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "4",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "-4",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Trong các khẳng định sau khẳng định không đúng là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "-1 < 0",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "1 > 0",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "-2 < -3",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "-3 < -2",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tập hợp các ước của 9 là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "{0; 1; 3; 9}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "{1; 3; 9}",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "{1; 3; 6}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "{1; 3}",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Trong các số sau, số nào chia hết cho cả 3 và 5?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "6",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "24",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "17",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "15",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Số nghịch đảo của  4/7  là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "4/-7",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "-4/7",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "7/4",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "-7/4",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Kết quả của phép tính 3.(−5).(−8) là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "−120",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "-39",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "16",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "120",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Các cặp phân số bằng nhau là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "-3/4 và -4/3",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "-2/3 và 6/9",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "3/7 và -3/7",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "7/8 và -35/-40",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Kết quả rút gọn phân số -210/300 đến tối giản là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "-21/30",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "21/30",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "-7/10",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "7/10",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "So sánh hai phân số -3/4 và 4/-5:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "-3/4 = 4/-5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "-3/4 < 4/-5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "-3/4 > 4/-5",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "-3/4  ≥ 4/-5",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Số đối của 5/11 là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "5/11",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "-5/11",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "-11/5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "11/5",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Với a = 4; b = -5 thì tích a2b bằng:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "80",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "-40",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "100",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "-100",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Số phần tử của tập hợp A = {2; 3; 4; 7; 8} là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "4",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "5",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "6",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "7",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Số liền sau số 17 là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "16",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "17",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "18",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "19",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Trong các số sau: 123; 35; 27; 84 số nào chia hết cho 2?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "123",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "35",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "27",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "84",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Kết quả của phép tính: (-17) + 23 + (-6) là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "0",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "-6",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "23",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "-17",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Trong các số sau: 12; 24; 31; 56 số nào là số nguyên tố?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "56",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "31",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "24",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "12",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Cho số a thuộc N* ta có kết quả phép tính 0 : a bằng:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "0",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "1",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "a",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Không thực hiện được",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tìm số nguyên P sao cho P + 3 và P + 5 là cùng số nguyên tố?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "P = 2",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "P = 3",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "P = 5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "P = 7",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tìm số nguyên x, biết -7/11  = -28/x.",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "x = 10",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "x = 11",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "x = 44",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "x = -44",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Cho tổng M = 9 + 72 + 2007 + x. Điều kiện của số tự nhiên x để M chia hết cho 9 là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "x chia cho 9 dư 1",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "x chia cho 9 dư 3",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "x chia cho 9 dư 6",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "x chia cho 9",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Điểm M là trung điểm của đoạn thẳng AB thì:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "MA > MB",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "MA < MB",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "MA = MB",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tất cả đều đúng",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Cho tập hợp A = {8; 12; 14}. Cách viết nào sau đây không đúng?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "14 ∈ A",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "{8; 12; 14} ⊂ A",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "{8; 14} ⊂ A",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "{12} ∈ A",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Gọi A là tập tập các số tự nhiên nhỏ hơn 5. Số phần tử của tập hợp A là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "4",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "5",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "6",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "7",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tập hợp nào sau đây không có phần tử nào:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Tập hợp các số tự nhiên x mà: x + 5 = 5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Tập hợp các số tự nhiên x mà x.0 = 0",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Tập hợp các số tự nhiên x mà x.0 = 1",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tập hợp các số tự nhiên x mà x - 0 = 1",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Kết qua của phép tính 120 - 60 : 5 - 4 là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "8",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "60",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "104",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "112",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Một cửa hàng có 7305 mét vải, cửa hàng đã bán đi 2183 mét vải. Số mét vải còn lại của cửa hàng là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "4122",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "5122",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "5022",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "5222",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Cho tập hợp M = {2 ; 4 ; 6}. Khẳng định nào sau đây là đúng?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Số 2 không phải là phần tử của tập hợp M",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Số 4 là phần tử của tập hợp M",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Số 6 không phải là phần tử của tập hợp M",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Số 3 là phần tử của tập hợp M",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tập hợp {x ∈ N, x < 5} còn có cách viết khác là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "{1 ; 2 ; 3 ; 4 ; 5}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "{0 ; 1 ; 2 ; 3 ; 4 ; 5}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "{1 ; 2 ; 3 ; 4}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "{0 ; 1 ; 2 ; 3 ; 4}",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Tập hợp P = {x ∈ N ⎢x ≤ 10} gồm bao nhiêu phần tử?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "12",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "11",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "10",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "9",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "<p>Cho hai tập hợp P = {a ; b; p; 0; 1} và Q = { b ; d ; m ; 1 ; 2}.</p><p>Tập hợp M các phần tử thuộc Q mà không thuộc P là:</p>",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "M = {a ; p ; 1}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "M = {d ; m ; 1}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "M = {d ; m}",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "M = {d ; m ; 2}",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Gọi M là tập hợp các số tự nhiên nhỏ hơn 7. Cách viết nào sau đây <b>không đúng</b>?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "0 ∈ M",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "1 ∈ M",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "8 ∉ M",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "7 ∈ M",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Cho tập hợp M các số tự nhiên x là số chẵn và 4 ≤ x < 12, điền kí hiệu ∈ ⊂ ∉ , , hoặc = vào chỗ chấm (...) cho đúng.",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "12 ... M",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "6 ... M",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "{4 ; 6 ; 8 ; 10} ... M",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "{4 ; 8 ; 10} ... M",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Đẳng thức nào sau đây thể hiện tính chất giao hoán của phép cộng các số tự nhiên (với m, n, p là các số tự nhiên)?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "m + (n + p) = (m + n) + p",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "m . (n + p) = m . n + m . p",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "m + n = n + m",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "(m + n) . p = m . p + n . p.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Số tự nhiên có số chục là 230, chữ số hàng đơn vị là 0 được viết là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "230",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "2030",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "2300",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "23",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Số nhỏ nhất trong các số 6537 ; 6357 ; 6735 ; 6375 là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "6537",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "6357",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "6735",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "6375",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Dấu <b>></b> điền vào [ ] nào sau đây là đúng?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "4 . 7 [ ] 5 . 6",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "12 + 19 [ ] 13 + 17",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "4 + 3 . 2 [ ] 22 − 9",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "3 . 5 + 1 [ ] 4 . 4",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Dấu <b>≠</b> điền vào [ ]  nào sau đây là đúng?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "4 . 7 [ ] 7 . 4",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "6 + 2 . 3 [ ] 12",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "2 + 6 . 3 [ ] (2 + 6) . 3 ",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "3 . 6 + 1 [ ] 19",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Dấu <b><</b> điền vào [ ] nào sau đây là đúng?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "2 + 4 . 5 [ ] (2 + 4) . 5",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "15 − 9 [ ] 13 − 7",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "4 + 23 [ ] 26",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "3 . 5 + 1 [ ] 3 . 5",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Theo cách ghi trong hệ La Mã, số IX được đọc là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "bốn",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "sáu",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "chín",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "mười một",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Số tự nhiên 16 được viết bằng số La Mã là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "XIV",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "XVI",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "IVX",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "VIX",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Kết quả dãy tính 80 − 40 : 5 − 4 là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "76",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "68",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "40",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "4",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Kết quả dãy tính 15 + (35 − 10 : 5) là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "48",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "38",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "20",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "8",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tổng ba số chẵn liên tiếp bằng 48. Số lớn nhất trong ba số đó là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "14",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "16",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "18",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "20",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Một mảnh đất hình chữ nhật có chiều dài 12m, chiều rộng kém chiều dài 5m. Chu vi mảnh đất đó là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "19 m",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "34 m",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "38 m",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "58 m",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Kết quả của phép tính 12.25 − 25.8 là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "2200",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "100",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "80",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "0",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Phép chia 379 cho 12 có số dư là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "9",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "7",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "3",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Số tự nhiên x lớn nhất sao cho x < (898 : 16) là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "58",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "57",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "56",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "55",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Số nào sau đây là số nguyên tố?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "1",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "3",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "6",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "9",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tập hợp nào sau đây chỉ gồm các số nguyên tố?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "{13 ; 15 ; 17 ; 19}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "{1 ; 2 ; 5 ; 7}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "{3 ; 5 ; 7 ; 11}",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "{3 ; 7 ; 9 ; 13}",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Số nào sau đây là hợp số?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "1",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "6",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "7",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tập hợp các số tự nhiên là ước của 16 là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "{2 ; 4 ; 8}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "{2 ; 4 ; 8 ; 16}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "{1 ; 2 ; 4 ; 6 ; 8 ; 16}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "{1 ; 2 ; 4 ; 8 ; 16}",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Tập hợp các ước chung của 30 và 36 là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "{2 ; 3 ; 6}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "{1 ; 2 ; 3 ; 6}",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "{1 ; 2 ; 3 ; 4}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "{1 ; 2 ; 3 ; 6 ; 9}",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Số nào sau đây chia hết cho cả 2 và 3?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "326",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "252",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "214",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "182",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Số 7155:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "không chia hết cho 9",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "chia hết cho cả 2 và 9",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "chia hết cho cả 3 và 9",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "chia hết cho 3 mà không chia hết cho 9",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Số nào sau đây là ước chung của 15 và 36?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "9",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "6",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "3",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Có bao nhiêu số x thoả mãn x ∈ Ư(36) và x > 4?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "6",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "4",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "3",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tập hợp các số tự nhiên là ước của 16 có bao nhiêu phần tử?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "4",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "3",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "2",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tập hợp các số tự nhiên là bội của 7 và nhỏ hơn 35 là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "{0 ; 7 ; 14 ; 21 ; 28 ; 35}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "{0 ; 7 ; 14 ; 21 ; 28}",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "{7 ; 14 ; 21 ; 28}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "{0 ; 7 ; 14 ; 28}",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Số nào sau đây là bội chung của 4 và 6?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "2",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "12",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "16",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "18",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Tập hợp các ước của 64 là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "{2 ; 4 ; 8 ; 16 ; 32}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "{2 ; 4 ; 8 ; 16 ; 32 ; 64}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "{1 ; 2 ; 4 ; 8 ; 16 ; 32 ; 64}",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "{1 ; 2 ; 4 ; 6 ; 8 ; 16 ; 32 ; 64}",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "ƯCLN(18, 120) là",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "60",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "12",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "9",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "6",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "BCNN(4, 18) là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "72",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "54",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "36",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "18",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Hai số nào sau đây có tích bằng 216 và có ước chung lớn nhất bằng 6?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "9 và 24",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "12 và 18",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "18 và 24",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "6 và 24",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "<p>Khẳng định nào sau đây không đúng?</p><p>Cho tập hợp M x 4 x 3 = ∈ − ≤ < { Z }. Khi đó trrong tập M:</p>",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Các số nguyên âm là : – 4 ; – 3 ; – 2 ; – 1 ; 0",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Các số tự nhiên là : 0 ; 1 ; 2",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Các số nguyên dương là : 1 ; 2",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Các số nguyên là : – 4 ; – 3 ; – 2 ; – 1 ; 0 ; 1 ; 2",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "<p>Khẳng định nào sau đây đúng?</p><p>Cho các tập hợp số M = {– 3 ; – 2 ; –1 ; 0 ; 1 ; 2 ; 3} và N = { x ∈ Z|-3 < x < 3 }.</p><p>Khi đó</p>",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Các số nguyên âm thuộc M là : – 3 ; – 2 ; – 1 ; 0",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Các số nguyên dương thuộc N là : 0 ; 1 ; 2",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Các số nguyên thuộc M ∩ N là : – 2 ; –1 ; 0 ; 1 ; 2",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Các số nguyên thuộc M ∩ N là : – 3 ; – 2 ; – 1 ; 0 ; 1 ; 2 ; 3",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Kết luận nào sau đây <b>không đúng</b>?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Tập ước của 9 là : {– 9 ; – 3 ; – 1 ; 0 ; 1 ; 3 ; 9}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Tập bội của 6 là : {... ; – 12 ; – 6 ; 0 ; 6 ; 12 ; ...}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Số lớn nhất khác 9 trong tập hợp ước của 9 là 3",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Số dương nhỏ nhất khác 0 trong tập bội của 6 là 6",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "<p>Kết luận nào sau đây đúng?</p><p>Giao của tập ước của 6 và tập ước của 15 là:</p>",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "{1 ; 3}",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "{– 3 ; – 1}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "{– 3 ; – 1 ; 1 ; 3}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "{– 15 ; – 6 ; – 5 ; – 3 ; – 1 ; 1 ; 3 ; 5 ; 6 ; 15}",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "<p>Kết quả nào sau đây đúng?</p><p>Với mọi x, y , ∈ Z đặt T = 21x + 2010y, khi đó:</p>",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "T chia hết cho 2",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "T chia hết cho 3",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "T chia hết cho 5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "T chia hết cho 7",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Khẳng định nào sau đây <b>không đúng</b>?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Mọi số tự nhiên đều có số liền sau",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Mọi số nguyên đều có số liền trước",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Giữa hai số nguyên trên trục số đều có một số nguyên khác nằm giữa",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tập hợp số nguyên Z gồm tập các số nguyên âm và tập các số nguyên dương và số 0",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Khẳng định nào sau đây <b>không đúng</b>?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Tổng của hai số nguyên dương là một số nguyên dương",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Tổng của hai số nguyên âm là một số nguyên âm",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Tổng của hai số nguyên trái dấu là một số nguyên âm",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tổng của hai số nguyên cùng dấu là một số nguyên dương hoặc là một số nguyên âm",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = " Khẳng định nào sau đây <b>không đúng</b>?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Tích của hai số nguyên dương là một số nguyên dương",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Tích của hai số nguyên âm là một số nguyên âm",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Tích của hai số nguyên trái dấu là một số nguyên âm",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tích của hai số nguyên bằng 0 khi và chỉ khi ít nhất một trong hai số nguyên đó bằng 0",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Kết quả nào sau đây <b>không đúng</b>?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "(– 47) + 3 . (36 – 17) = 10",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "(– 19) – 28 . (– 2) + 14 = 51",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "(– 4) . [29 – 3 . (–19 – 25)] + 489 = 155",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "1 – {2 – [3 – (4 – 5)]} = 3",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "<p>Khẳng định nào sau đây không đúng?</p><p>Với a, b∈ Z , khi đó:</p>",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Hiệu a – (– b) là một số nguyên dương khi a dương và b dương",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Hiệu (– a) – b là một số nguyên âm",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Hiệu a – b là một số nguyên âm nếu a âm và b dương",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Hiệu a – b là một số nguyên dương nếu a dương và b âm",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Khẳng định nào sau đây <b>không đúng</b>?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Số đối của 2 là –2",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Số đối của số nguyên x là –x",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Số đối của |−3| là 3",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Số đối của |4| là –4",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Kết quả nào sau đây <b>không đúng</b>?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "(– 14) . 0 = 0",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "(– 7) . (– 8) = 56",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "6 . (–12) = –72",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "(– 4) . (– 5) . (– 6) = 120",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "<p>Khẳng định nào sau đây đúng?</p><p>Cho (123 – x) . [(–12) . x + (–7) . (–24)] = 0, khi đó:</p>",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "x ∈ {–14 ; 123}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "x ∈ {-123 ; 14}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "x ∈ {14 ; 123}",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "x ∈ {-123 ; -14}",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Kết quả nào sau đây <b>không đúng</b>?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "2008 – (2001 + 2007) = 2000",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "(– 1999 + 2001) – 2009 = – 2007",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "2006 – 2010 + (– 2001) = – 2005",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "(– 1991) – (–1992) – 1993 = – 1992",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Kết quả của phép tính 3.(−5).(−8) là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "−120",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "−39",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "16",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "120",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Các cặp phân số bằng nhau là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "-3/4 và -4/3",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "-2/3 và 6/9",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "3/7 và -3/7",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "7/8 và -35/-40",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Kết quả rút gọn phân số-210/300 đến tối giản là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "-21/30",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "21/30",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "-7/10",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "7/10",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "So sánh hai phân số -3/4 và 4/-5",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "-3/4 = 4/-5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "-3/4 < 4/-5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "-3/4 > 4/-5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "-3/4 >= 4/-5",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Số đối của 5/11 là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "5/11",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "-5/11",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "-11/5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "11/5",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Phân số 2/5 viết dưới dạng phần trăm là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "13/3",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "2.5%",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "4%",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "40%",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Lớp 6A có 40 học sinh trong đó có 12,5% là học sinh giỏi. Số học sinh giỏi của lớp 6A là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "6",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "8",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "10",
                            DSS = false
                        }
                    }
                }
            };
        }

        private List<Question> GetQuestionsKhoi6HinhHoc()
        {
            var type = (int)QuestionType.SingleChoice;
            var currentDate = DateTime.UtcNow.Date;

            return new List<Question>
            {
                new Question
                {
                    Content = "Cho các đoạn thẳng AB, CD, EF. Cho biết CD = 7 cm, EF = 5 cm, số đo độ dài AB là số tự nhiên, AB < CD, AB > EF. Vậy AB = ?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "8 cm",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "6 cm",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "4 cm",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "12 cm",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Cho điểm A nằm giữa hai điểm B và C, điểm M nằm giữa hai điểm A và B, điểm N nằm giữa hai điểm A và C. Ta có:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Hai tia AM và AB trùng nhau.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Hai tia AN và AC trùng nhau.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "A nằm giữa hai điểm M và N.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "M nằm giữa hai điểm A và N",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Khi 2 điểm M và N trùng nhau, ta nói khoảng cách giữa M và N bằng:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "0",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "1",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "-2",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "3",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Trên tia Ox, hãy vẽ hai đoạn thẳng OM và ON biết OM = 3 cm và ON = 5 cm. Trong 3 điểm O, M, N, điểm nằm giữa hai điểm còn lại là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "O",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "M",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "N",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Không có điểm nào.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Chọn câu trả lời đúng:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Số đo độ dài đoạn thẳng là một số tự nhiên.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Số đo độ dài đoạn thẳng là một số lẻ.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Số đo độ dài đoạn thẳng là một số chẵn.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Số đo độ dài đoạn thẳng là một số dương.",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Cho 10 điểm. Cứ qua hai điểm vẽ một đoạn thẳng. Số đoạn thẳng vẽ được tất cả là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "5",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "11",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "20",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "45",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Cho 2 tia Ox và Oy đối nhau, các điểm H, K thuộc tia Ox (K nằm giữa O và H), điểm G thuộc tia Oy. Tia đối của tia OG là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Tia OK hoặc tia OH hoặc tia Ox (ba tia này trùng nhau).",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Tia Oy",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Tia Kx",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tia Hx",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Chọn câu trả lời đúng:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Có hai đường thẳng đi qua hai điểm A và B.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Có vô số đường thẳng đi qua hai điểm A và B",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Có một đường thẳng và chỉ một đường thẳng đi qua hai điểm A và B.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Có nhiều hơn một đường thẳng đi qua hai điểm B và C.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Nếu điểm M thuộc đường thẳng d thì:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Điểm M nằm trên đường thẳng d.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Đường thẳng d đi qua điểm M",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Đường thẳng d chứa điểm M",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Đường thẳng d không chứa điểm M",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Cho đoạn thẳng AB dài 4cm, gọi M là trung điểm của AB. Khi đó, MA dài:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "4 cm",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "8 cm",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "2 cm",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "1 cm",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Trên tia Ox lấy điểm A, B sao cho OA = 3cm, OB = 2,5cm. Khi đó có hai tia đối nhau là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "BA và BO",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "OB và OA",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "AO và AB",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "AB và BA",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Kết luận nào sau đây là đúng?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Hai góc kề nhau có tổng số đo bằng 90 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Hai góc phụ nhau có tổng số đo bằng 180 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Hai góc bù nhau có tổng số đo bằng 90 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Hai góc bù nhau có tổng số đo bằng 180 độ",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Cho hai góc bù nhau, trong đó có một góc bằng 35 độ. Số đo góc còn lại sẽ là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "65 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "55 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "145 độ",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "165 độ",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Điểm I là trung điểm của đoạn thẳng AB có độ dài 8cm, vậy độ dài đoạn thẳng IA bằng bao nhiêu?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "2 cm",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "4 cm",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "6 cm",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "8 cm",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "<p>Trên tia Ox, vẽ hai điểm A, B sao cho: OA = 2 cm; OB = 4 cm</p><p>Điểm A có nằm giữa hai điểm O và B không?</p>",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Có",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Không",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "<p>Trên tia Ox, vẽ hai điểm A, B sao cho: OA = 2 cm; OB = 4 cm</p><p>Điểm A có là trung điểm của đoạn thẳng OB không?</p>",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Có",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Không",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Có bao nhiêu đường thẳng đi qua hai điểm phân biệt?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Không có đường thẳng nào.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Có một đường thẳng.",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Có hai đường thẳng.",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Có ba đường thẳng",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Hai đường thẳng phân biệt là hai đường thẳng:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Không có điểm chung",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Có 1 điểm chung",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Có 2 điểm chung",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Có 1 điểm chung hoặc không có điểm chung nào",
                            DSS = true
                        }
                    }
                },
                new Question
                {
                    Content = "Để đặt tên cho 1 tia, người ta thường dùng:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Hai chữ cái thường",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Một chữ cái viết thường",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Một chữ cái viết hoa",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Một chữ cái viết hoa làm gốc và một chữ viết thường.",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Trên tia Ox có OA = 5cm, OB = 3cm thì:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Điểm B nằm giữa O và A",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Điểm A nằm giữa O và B",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Điểm O nằm giữa A và B",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Tất cả đều đúng",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Cho hai đường thẳng cắt nhau tại một điểm. Số góc tạo bởi hai đường thẳng cắt nhau là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "4 góc",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "5 góc",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "6 góc",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "7 góc",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Lúc 6 giờ 00 phút, số đo góc tạo bởi kim giờ và kim phút là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "180 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "120 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "90 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "45 độ",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "<p>Cho (O ; 2,5 cm). Biết OM = 1cm ; ON = 2,5cm ; OP = 3cm ; OQ = 2cm.</p><p>Khi đó số điểm nằm trong đường tròn tâm O là:</p>",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "1 điểm",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "2 điểm",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "3 điểm",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "4 điểm",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Kết luận nào sau đây là đúng?",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Hai góc kề nhau có tổng số đo bằng 90 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Hai góc phụ nhau có tổng số đo bằng 180 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Hai góc bù nhau có tổng số đo bằng 90 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Hai góc bù nhau có tổng số đo bằng 180 độ",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Cho hai góc bù nhau, trong đó có một góc bằng 35 độ. Số đo góc còn lại sẽ là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "65 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "55 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "145 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "165 độ",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Góc vuông là góc có số đo:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Bằng 180 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Nhỏ hơn 90 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Bằng 90 độ",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Lớn hơn 90 độ",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Góc 30 độ phụ với góc có số đo bằng:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "0 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "60 độ",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "90 độ",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "180 độ",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Biết rằng MNP = 180 độ câu nào sau đây <b>không đúng</b>",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Ba điểm M, N, P thẳng hàng",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Hai tia MP và MN đối nhau",
                            DSS = true
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Hai tia NP và NM đối nhau",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Góc MNP là góc bẹt",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Nếu xOy + yOz = xOz thì:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Tia Oz nằm giữa hai tia Ox và Oy",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Tia Ox nằm giữa hai tia Oz và Oy",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Tia Oy nằm giữa hai tia Ox và Oz",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Không có tia nào nằm giữa hai tia còn lại",
                            DSS = false
                        }
                    }
                },
                new Question
                {
                    Content = "Hình gồm các điểm cách đều điểm I một khoảng cách IA = 3cm là:",
                    Type = type,
                    Specialized = false,
                    Month = currentDate,
                    Answers = new List<Answer>
                    {
                        new Answer
                        {
                            AnswerName = "A",
                            Body = "Tia IA",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "B",
                            Body = "Đường tròn tâm I bán kính 3cm",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "C",
                            Body = "Đoạn thẳng IA",
                            DSS = false
                        },
                        new Answer
                        {
                            AnswerName = "D",
                            Body = "Cả A; B; C đều đúng",
                            DSS = false
                        }
                    }
                }
            };
        }

        #endregion
    }
}
