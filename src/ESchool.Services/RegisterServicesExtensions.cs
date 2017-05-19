using ESchool.Services.Examinations;
using ESchool.Services.Files;
using ESchool.Services.Infrastructure;
using ESchool.Services.Infrastructure.Cache;
using ESchool.Services.Messages;
using ESchool.Services.Systems;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ESchool.Services
{
    public static class RegisterServicesExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfigurationRoot configuration)
        {
            // If you need access to generic IConfiguration this is required.
            services.AddSingleton<IConfiguration>(x => configuration);

            // Add functionality to inject IOptions<T>.
            services.AddOptions();

            services.Configure<AppSettings>(appSettings =>
            {
                appSettings.MemoryCacheInMinutes = int.Parse(configuration["AppSettings:MemoryCacheInMinutes"]);
                appSettings.ServerUploadFolder = configuration["AppSettings:ServerUploadFolder"];
            });

            // Infrastructure.
            services.AddScoped<IMemoryCacheService, MemoryCacheService>();

            // Systems.
            services.AddScoped<ILogService, LogService>();

            // Files.
            services.AddScoped<IFileService, FileService>();

            // Messages
            services.AddScoped<IEmailAccountService, EmailAccountService>();
            services.AddScoped<IQueuedEmailService, QueuedEmailService>();
            services.AddScoped<IEmailSender, MailKitEmailSender>();

            // Examinations.
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IQTagService, QTagService>();
            services.AddScoped<IQuestionService, QuestionService>();

            return services;
        }
    }
}
