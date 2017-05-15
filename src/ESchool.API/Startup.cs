using ESchool.Admin.Attributes;
using ESchool.API.Extensions;
using ESchool.Data.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

namespace ESchool.API
{
    public class Startup
    {
        private IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            // Load from Database.
            builder.AddEntityFrameworkConfig(opts => opts.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            // Set our site-wide config.
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Enable CORS.
            var corsBuilder = new CorsPolicyBuilder()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials();
            services.AddCors(opts =>
            {
                opts.AddPolicy("AllowAllOrigins", corsBuilder.Build());
            });

            services.AddCustomAuthorization(Configuration.GetConnectionString("DefaultConnection"));

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddMemoryCache();
            //services.AddDistributedMemoryCache();
            //services.AddSession(opts => opts.IdleTimeout = TimeSpan.FromMinutes(20));

            // Add framework services.
            services
                .AddMvc(opts =>
                {
                    opts.Filters.Add(typeof(GlobalExceptionFilterAttribute));
                })
                .AddJsonOptions(opts =>
                {
                    var serializerSettings = opts.SerializerSettings;

                    serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    serializerSettings.Formatting = Formatting.None;
                    serializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddApplicationService(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory
                .AddConsole(Configuration.GetSection("Logging"))
                .AddDebug()
                .AddNLog();

            app.AddNLogWeb();

            var variables = LogManager.Configuration.Variables;
            variables["connectionString"] = Configuration.GetConnectionString("DefaultConnection");
            variables["configDir"] = "Logs";
            // ----- End of NLog -----

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAllOrigins");

            app.UseCustomAuthorization();

            app.UseMvcWithDefaultRoute();

            app.UseDefaultData()
                .UseBackgroundTasks();
        }
    }
}
