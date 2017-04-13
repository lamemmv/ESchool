using ESchool.Services.Examinations;
using ESchool.Services.Infrastructure.Cache;
using ESchool.Services.Systems;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ESchool.Services.Infrastructure.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfigurationRoot configuration)
        {
			// If you need access to generic IConfiguration this is required.
			services.AddSingleton<IConfiguration>(x => configuration);

            // Add functionality to inject IOptions<T>.
            services.AddOptions();

            //services.Configure<AppSettings>(appSettings =>
            //{
            //    appSettings.MemoryCacheInMinutes = int.Parse(configuration["AppSettings:MemoryCacheInMinutes"]);
            //});

            // Infrastructure.
            services.AddScoped<IMemoryCacheService, MemoryCacheService>();

            // Systems.
            services.AddScoped<ILogService, LogService>();

            // Examinations.
            services.AddScoped<IQTagService, QTagService>();
            services.AddScoped<IQuestionService, QuestionService>();

            return services;
        }
    }
}
