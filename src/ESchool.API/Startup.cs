using System.IdentityModel.Tokens.Jwt;
using AspNet.Security.OpenIdConnect.Primitives;
using ESchool.Admin.Attributes;
using ESchool.API.Extensions;
using ESchool.Data;
using ESchool.Data.Configurations;
using ESchool.Domain.Entities.Systems;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
                .AddDebug();

            ConfigureNLog(app, loggerFactory);

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAllOrigins");

            // Add a middleware used to validate access
            // tokens and protect the API endpoints.
            //app.UseOAuthValidation();

            // If you prefer using JWT, don't forget to disable the automatic
            // JWT -> WS-Federation claims mapping used by the JWT middleware:
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                Authority = "http://localhost:59999/",
                Audience = "http://localhost:59999/",
                //AutomaticAuthenticate = true,
                //AutomaticChallenge = true,
                RequireHttpsMetadata = false,
                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = OpenIdConnectConstants.Claims.Subject,
                    RoleClaimType = OpenIdConnectConstants.Claims.Role
                }
            });

            // Alternatively, you can also use the introspection middleware.
            // Using it is recommended if your resource server is in a
            // different application/separated from the authorization server.
            // app.UseOAuthIntrospection(options =>
            // {
            //     options.Authority = new Uri("http://localhost:59999/");
            //     options.Audiences.Add("resource_server");
            //     options.ClientId = "eschool.web";
            //     options.ClientSecret = "eschool.web.P@$$w0rd";
            //     options.RequireHttpsMetadata = false;

            app.UseOpenIddict();

            app.UseMvcWithDefaultRoute();

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
