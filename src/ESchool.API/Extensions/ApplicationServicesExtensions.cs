using AutoMapper;
using ESchool.Services.Infrastructure.Cache;
using ESchool.Services.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ESchool.API.Extensions
{
	public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfigurationRoot configuration, MapperConfiguration autoMapperConfiguration)
        {
			// 3rd services:
			// AutoMapper.
			services.AddSingleton(cfg => autoMapperConfiguration.CreateMapper());

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

			// Logs,
			services.AddScoped<ILogService, LogService>();

			return services;
        }
    }
}
