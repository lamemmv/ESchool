using System.Threading.Tasks;
using ESchool.Data;
using ESchool.Data.Entities.Accounts;
using ESchool.Services.Infrastructure.Tasks;
using ESchool.Services.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Core;
using OpenIddict.Models;

namespace ESchool.Services.AppStart
{
    public static class RegisterStartupTasksExtensions
    {
        public static IApplicationBuilder UseStartupTasks(this IApplicationBuilder app)
        {
            InitDefaultData(app);

            InitBackgroundTasks(app);

            return app;
        }

        private static void InitDefaultData(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var dbContext = serviceProvider.GetRequiredService<ObjectDbContext>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var applicationManager = serviceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

                var dbInitializer = new DbInitializer();

                // This protects from deadlocks by starting the async method on the ThreadPool.
                Task.Run(() => dbInitializer.Initialize(dbContext, roleManager, userManager, applicationManager)).Wait();
            }
        }

        private static void InitBackgroundTasks(IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;

            var logger = serviceProvider.GetRequiredService<ILogger<QueuedEmailSendTask>>();
            var queuedEmailService = serviceProvider.GetRequiredService<IQueuedEmailService>();
            var emailSender = serviceProvider.GetRequiredService<IEmailSender>();

            IBackgroundTask emailSenderTask = new QueuedEmailSendTask(1, 5, logger, queuedEmailService, emailSender);
            emailSenderTask.Start();
        }
    }
}
