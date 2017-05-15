using ESchool.Data;
using ESchool.Domain.Entities.Accounts;
using ESchool.Services.Infrastructure.Tasks;
using ESchool.Services.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ESchool.API.Extensions
{
    public static class RegisterStartupTasksExtensions
    {
        public static IApplicationBuilder UseDefaultData(this IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;

            var dbContext = serviceProvider.GetRequiredService<ObjectDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var dbInitializer = new DbInitializer();
            dbInitializer.Initialize(dbContext, roleManager, userManager);

            return app;
        }

        public static IApplicationBuilder UseBackgroundTasks(this IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;
            var queuedEmailService = serviceProvider.GetRequiredService<QueuedEmailService>();
            var emailSender = serviceProvider.GetRequiredService<MailKitEmailSender>();

            IBackgroundTask emailSenderTask = new QueuedEmailSendTask(1, 5, queuedEmailService, emailSender);
            emailSenderTask.Start();

            return app;
        }
    }
}
