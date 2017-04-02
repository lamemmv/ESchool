using System;
using ESchool.Data;
using ESchool.Domain.Entities.Systems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ESchool.Services.Infrastructure.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
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
                var cookieOpts = opts.Cookies.ApplicationCookie;
                //cookie.AccessDeniedPath = "";
                //cookie.AuthenticationScheme = "osCookie";
                //cookie.CookieName = ".osIdentity";
                //cookie.CookiePath = "/";
                //cookie.DataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo("C:\\Github\\Identity\\artifacts"));
                cookieOpts.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                cookieOpts.LoginPath = new PathString("/admin/Authentication/SignIn");
                cookieOpts.LogoutPath = new PathString("/admin/Authentication/SignOut");
                cookieOpts.AccessDeniedPath = new PathString("/admin/Error/Forbidden");

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

            return services;
        }
    }
}
