using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using AspNet.Security.OpenIdConnect.Primitives;
using ESchool.Data;
using ESchool.Data.Entities.Accounts;
using ESchool.Services.Authorizations;
using ESchool.Services.Constants;
using ESchool.Services.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ESchool.Services.AppStart
{
    public static class RegisterAuthorizationExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services, string connectionString)
        {
            string migrationsAssembly = typeof(ObjectDbContext).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<ObjectDbContext>(opts =>
            {
                opts.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly));

                // Register the entity sets needed by OpenIddict.
                // Note: use the generic overload if you need
                // to replace the default OpenIddict entities.
                opts.UseOpenIddict();
            });

            // Register the Identity services.
            services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
            {
                // Password settings.
                PasswordOptions passwordOpts = opts.Password;
                passwordOpts.RequireDigit = false;
                passwordOpts.RequiredLength = 6;
                passwordOpts.RequireNonAlphanumeric = false;
                passwordOpts.RequireUppercase = false;
                passwordOpts.RequireLowercase = false;

                // Lockout settings.
                LockoutOptions lockoutOpts = opts.Lockout;
                lockoutOpts.AllowedForNewUsers = true;
                lockoutOpts.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                lockoutOpts.MaxFailedAccessAttempts = 5;

                // Cookie settings.
                //var cookieOpts = opts.Cookies.ApplicationCookie;
                //cookie.AccessDeniedPath = "";
                //cookie.AuthenticationScheme = "osCookie";
                //cookie.CookieName = ".osIdentity";
                //cookie.CookiePath = "/";
                //cookie.DataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo("C:\\Github\\Identity\\artifacts"));
                //cookieOpts.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                //cookieOpts.LoginPath = new PathString("/admin/Authentication/SignIn");
                //cookieOpts.LogoutPath = new PathString("/admin/Authentication/SignOut");
                //cookieOpts.AccessDeniedPath = new PathString("/admin/Error/Forbidden");

                // User settings.
                //options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
                opts.User.RequireUniqueEmail = true;

                // SignIn settings.
                SignInOptions signinOpts = opts.SignIn;
                signinOpts.RequireConfirmedEmail = true;
                signinOpts.RequireConfirmedPhoneNumber = false;
            })
                .AddEntityFrameworkStores<ObjectDbContext>()
                .AddDefaultTokenProviders();

            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            services.Configure<IdentityOptions>(opts =>
            {
                ClaimsIdentityOptions claimsIdentityOpts = opts.ClaimsIdentity;
                claimsIdentityOpts.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                claimsIdentityOpts.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                claimsIdentityOpts.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            // Register the OpenIddict services.
            services.AddOpenIddict(opts =>
            {
                // Register the Entity Framework stores.
                opts.AddEntityFrameworkCoreStores<ObjectDbContext>();

                // Register the ASP.NET Core MVC binder used by OpenIddict.
                // Note: if you don't call this method, you won't be able to
                // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                opts.AddMvcBinders();

                // Enable the token endpoint.
                opts.EnableAuthorizationEndpoint("/connect/authorize")
                    .EnableLogoutEndpoint("/connect/logout")
                    .EnableTokenEndpoint("/connect/token");

                // Enable the password flow.
                opts.AllowPasswordFlow()
                    .AllowRefreshTokenFlow();

                opts.SetAccessTokenLifetime(TimeSpan.FromHours(1))
                    .SetRefreshTokenLifetime(TimeSpan.FromDays(14));

                // Make the "client_id" parameter mandatory when sending a token request.
                opts.RequireClientIdentification();

                // When request caching is enabled, authorization and logout requests
                // are stored in the distributed cache by OpenIddict and the user agent
                // is redirected to the same page with a single parameter (request_id).
                // This allows flowing large OpenID Connect requests even when using
                // an external authentication provider like Google, Facebook or Twitter.
                //opts.EnableRequestCaching();

                // During development, you can disable the HTTPS requirement.
                opts.DisableHttpsRequirement();

                // Note: to use JWT access tokens instead of the default
                // encrypted format, the following lines are required:
                opts.UseJsonWebTokens();
                opts.AddEphemeralSigningKey();
            });

            return services;
        }

        public static IServiceCollection AddCustomPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy(
                    ApiClaimTypes.ManageAccountsRead,
                    policy => policy.Requirements.Add(new ApiClaimRequirement(ApiClaimTypes.ManageAccountsRead, Permissions.Read)));
                opts.AddPolicy(
                    ApiClaimTypes.ManageAccountsCreate,
                    policy => policy.Requirements.Add(new ApiClaimRequirement(ApiClaimTypes.ManageAccountsCreate, Permissions.ReadCreate)));
                opts.AddPolicy(
                    ApiClaimTypes.ManageAccountsUpdate,
                    policy => policy.Requirements.Add(new ApiClaimRequirement(ApiClaimTypes.ManageAccountsUpdate, Permissions.ReadUpdate)));
                opts.AddPolicy(
                    ApiClaimTypes.ManageAccountsDelete,
                    policy => policy.Requirements.Add(new ApiClaimRequirement(ApiClaimTypes.ManageAccountsDelete, Permissions.ReadDelete)));
            });

            services.AddSingleton<IAuthorizationHandler, ApiClaimHandler>();

            return services;   
        }

        public static IApplicationBuilder UseCustomAuthorization(this IApplicationBuilder app)
        {
            // Add a middleware used to validate access
            // tokens and protect the API endpoints.
            //app.UseOAuthValidation();

            // If you prefer using JWT, don't forget to disable the automatic
            // JWT -> WS-Federation claims mapping used by the JWT middleware:
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                Audience = "eschool.api", //"http://localhost:59999/"
                Authority = "http://localhost:59999/",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
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

            return app;
        }
    }
}
