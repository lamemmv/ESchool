﻿using System;
using System.Reflection;
using AspNet.Security.OpenIdConnect.Primitives;
using ESchool.Data;
using ESchool.Domain.Entities.Systems;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ESchool.API.Extensions
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services, string connectionString)
        {
            var migrationsAssembly = typeof(ObjectDbContext).GetTypeInfo().Assembly.GetName().Name;
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
                var passwordOpts = opts.Password;
                passwordOpts.RequireDigit = false;
                passwordOpts.RequiredLength = 6;
                passwordOpts.RequireNonAlphanumeric = false;
                passwordOpts.RequireUppercase = false;
                passwordOpts.RequireLowercase = false;

                // Lockout settings.
                var lockoutOpts = opts.Lockout;
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
                var signinOpts = opts.SignIn;
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
                var claimsIdentity = opts.ClaimsIdentity;
                claimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                claimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                claimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
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
                opts.EnableTokenEndpoint("/connect/token");

                // Enable the password flow.
                opts.AllowPasswordFlow();
                    //.AllowRefreshTokenFlow();

                // During development, you can disable the HTTPS requirement.
                opts.DisableHttpsRequirement();

                // Note: to use JWT access tokens instead of the default
                // encrypted format, the following lines are required:
                opts.UseJsonWebTokens();
                opts.AddEphemeralSigningKey();

                // When request caching is enabled, authorization and logout requests
                // are stored in the distributed cache by OpenIddict and the user agent
                // is redirected to the same page with a single parameter (request_id).
                // This allows flowing large OpenID Connect requests even when using
                // an external authentication provider like Google, Facebook or Twitter.
                //opts.EnableRequestCaching();
            });

            return services;
        }
    }
}
