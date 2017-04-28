using System.Collections.Generic;
using ESchool.Domain.Entities.Systems;
using ESchool.Services.Systems;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ESchool.Services.Infrastructure.Extensions
{
    public static class IdentityServerExtensions
    {
        private const string HostUrl = "http://localhost:52923";

        public static IServiceCollection AddCustomIdentityServer(this IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddTemporarySigningCredential()
                .AddInMemoryIdentityResources(GetIdentityResources())
                .AddInMemoryApiResources(GetApiResources())
                .AddInMemoryClients(GetClients())
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<IdentityWithAdditionalClaimsProfileService>();

            return services;
        }

        public static IdentityServerAuthenticationOptions GetIdentityServerAuthenticationOptions()
        {
            return new IdentityServerAuthenticationOptions
            {
                Authority = HostUrl + "/",
                AllowedScopes = new List<string> { "dataEventRecords" },
                ApiSecret = "dataEventRecordsSecret",
                ApiName = "dataEventRecords",
                AutomaticAuthenticate = true,
                SupportedTokens = SupportedTokens.Both,
                // TokenRetriever = _tokenRetriever,
                // Required if you want to return a 403 and not a 401 for forbidden responses.
                AutomaticChallenge = true,
            };
        }

        private static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("dataeventrecordsscope", new []
                {
                    "role",
                    "admin",
                    "user",
                    "dataEventRecords",
                    "dataEventRecords.admin" ,
                    "dataEventRecords.user"
                })
            };
        }

        private static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("dataEventRecords")
                {
                    ApiSecrets =
                    {
                        new Secret("dataEventRecordsSecret".Sha256())
                    },
                    Scopes =
                    {
                        new Scope
                        {
                            Name = "dataeventrecordsscope",
                            DisplayName = "Scope for the dataEventRecords ApiResource"
                        }
                    },
                    UserClaims = { "role", "admin", "user", "dataEventRecords", "dataEventRecords.admin", "dataEventRecords.user" }
                }
            };
        }

        private static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "singleapp",
                    ClientName = "singleapp",
                    AccessTokenType = AccessTokenType.Reference,
                    //AccessTokenLifetime = 600, // 10 minutes, default 60 minutes
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new List<string>
                    {
                         HostUrl
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                         HostUrl
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                         HostUrl
                    },
                    AllowedScopes = new List<string>
                    {
                        "openid",
                        "dataEventRecords",
                        "dataeventrecordsscope",
                        "securedFiles",
                        "securedfilesscope",
                        "role"
                    }
                }
            };
        }
    }
}
