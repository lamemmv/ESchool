using System;
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
                IServiceProvider serviceProvider = scope.ServiceProvider;

                ObjectDbContext dbContext = serviceProvider.GetRequiredService<ObjectDbContext>();
                RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var applicationManager = serviceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

                var dbInitializer = new DbInitializer();

                // This protects from deadlocks by starting the async method on the ThreadPool.
                Task.Run(() => dbInitializer.Initialize(dbContext, roleManager, userManager, applicationManager)).Wait();
            }
        }

        private static void InitBackgroundTasks(IApplicationBuilder app)
        {
            IServiceProvider serviceProvider = app.ApplicationServices;

            ILogger<QueuedEmailSendTask> logger = serviceProvider.GetRequiredService<ILogger<QueuedEmailSendTask>>();
            IQueuedEmailService queuedEmailService = serviceProvider.GetRequiredService<IQueuedEmailService>();
            IEmailSender emailSender = serviceProvider.GetRequiredService<IEmailSender>();

            IBackgroundTask emailSenderTask = new QueuedEmailSendTask(1, 5, logger, queuedEmailService, emailSender);
            emailSenderTask.Start();
        }
    }
}
