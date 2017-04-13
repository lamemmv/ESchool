using System.Reflection;
using ESchool.Admin.Attributes;
using ESchool.Data;
using ESchool.Data.Configurations;
using ESchool.Domain.Entities.Systems;
using ESchool.Services.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(ObjectDbContext).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<ObjectDbContext>(opts =>
                opts.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly)));

            services.AddCustomIdentity();
            services.AddCustomIdentityServer();

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

            //services.AddAuthorization(opts =>
            //{
            //	opts.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));
            //});

            services.AddApplicationService(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory
                .AddConsole(Configuration.GetSection("Logging"))
                .AddDebug();

            ConfigureNLog(app, loggerFactory);

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }

            app.UseIdentity();
            app.UseIdentityServer();
            //app.UseIdentityServerAuthentication(
            //	new IdentityServerAuthenticationOptions
            //	{
            //		Authority = "http://localhost:39789/",
            //		RequireHttpsMetadata = false,
            //		ApiName = "api1"
            //	});

            //app.UseSession();

            app.UseCors("AllowAllOrigins");

            app.UseMvc(/*routes =>
            {
                routes.MapRoute(
                    name: "area_default",
                    template: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            }*/);

            InitializeDefaultData(app);
        }

        private void ConfigureNLog(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            app.AddNLogWeb();

            var variables = LogManager.Configuration.Variables;
            variables["connectionString"] = Configuration.GetConnectionString("DefaultConnection");
            variables["configDir"] = "Logs";
        }

        private void InitializeDefaultData(IApplicationBuilder app)
        {
            var serviceProvider = app.ApplicationServices;

            var dbContext = serviceProvider.GetRequiredService<ObjectDbContext>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var dbInitializer = new DbInitializer();
            dbInitializer.Initialize(dbContext, roleManager, userManager);
        }
    }
}
