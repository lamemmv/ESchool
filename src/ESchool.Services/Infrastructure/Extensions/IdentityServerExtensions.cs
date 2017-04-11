using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.DependencyInjection;

namespace ESchool.Services.Infrastructure.Extensions
{
    public static class IdentityServerExtensions
    {
        public static IServiceCollection AddCustomIdentityServer(this IServiceCollection services)
        {
            //// Configure identity server with in-memory stores, keys, clients and scopes.
            //services.AddIdentityServer()
            //    .AddTemporarySigningCredential()
            //    //.AddInMemoryPersistedGrants()
            //    //.AddInMemoryIdentityResources(GetIdentityResources())
            //    .AddInMemoryApiResources(GetApiResources())
            //    .AddInMemoryClients(GetClients());
            ////.AddAspNetIdentity<ApplicationUser>()
            ////.AddInMemoryUsers(new List<InMemoryUser>());

            services.AddIdentityServer()
                .AddInMemoryClients(GetClients())
                .AddInMemoryIdentityResources(GetIdentityResources())
                .AddInMemoryApiResources(GetApiResources())
                .AddTestUsers(GetTestUsers())
                .AddTemporarySigningCredential();

            return services;
        }

        //private static IEnumerable<IdentityResource> GetIdentityResources()
        //{
        //    return new List<IdentityResource>
        //    {
        //        new IdentityResources.OpenId(),
        //        new IdentityResources.Profile(),
        //        new IdentityResources.Email(),
        //        new IdentityResource
        //        {
        //            Name = "role",
        //            UserClaims = new List<string> { "role" }
        //        }
        //    };
        //}

        //private static IEnumerable<ApiResource> GetApiResources()
        //{
        //    /*return new List<ApiResource> {
        //        new ApiResource
        //        {
        //            Name = "customAPI",
        //            DisplayName = "Custom API",
        //            Description = "Custom API Access",
        //            UserClaims = new List<string> { "role" },
        //            ApiSecrets = new List<Secret> { new Secret("scopeSecret".Sha256()) },
        //            Scopes = new List<Scope>
        //            {
        //                new Scope("customAPI.read"),
        //                new Scope("customAPI.write")
        //            }
        //        }
        //    };*/

        //    return new List<ApiResource>
        //    {
        //        new ApiResource("api1", "My API")
        //    };
        //}

        //private static IEnumerable<Client> GetClients()
        //{
        //    //return new List<Client>
        //    //{
        //    //    new Client
        //    //    {
        //    //        ClientId = "oauthClient",
        //    //        ClientName = "Example Client Credentials Client Application",
        //    //        AllowedGrantTypes = GrantTypes.ClientCredentials,
        //    //        ClientSecrets = new List<Secret>
        //    //        {
        //    //            new Secret("superSecretPassword".Sha256())
        //    //        },
        //    //        AllowedScopes = new List<string> {"customAPI.read"}
        //    //    }
        //    //};

        //    return new List<Client>
        //    {
        //        new Client
        //        {
        //            ClientId = "client",
        //            AllowedGrantTypes = GrantTypes.ClientCredentials,
        //            ClientSecrets =
        //            {
        //                new Secret("secret".Sha256())
        //            },
        //            AllowedScopes = { "api1" }
        //        }
        //    };
        //}

        private static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "oauthClient",
                    ClientName = "Example Client Credentials Client Application",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("superSecretPassword".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        "customAPI.read"
                    }
                },
                new Client
                {
                    ClientId = "openIdConnectClient",
                    ClientName = "Example Implicit Client Application",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                        "customAPI"
                    },
                    RedirectUris = new List<string>
                    {
                        "http://localhost:27629/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:27629"
                    }
                }
            };
        }

        private static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string>
                    {
                        "role"
                    }
                }
            };
        }

        private static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource
                {
                    Name = "customAPI",
                    DisplayName = "Custom API",
                    Description = "Custom API Access",
                    UserClaims = new List<string>
                    {
                        "role"
                    },
                    ApiSecrets = new List<Secret>
                    {
                        new Secret("scopeSecret".Sha256())
                    },
                    Scopes = new List<Scope>
                    {
                        new Scope("customAPI.read"),
                        new Scope("customAPI.write")
                    }
                }
            };
        }

        private static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "scott",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Email, "scott@scottbrady91.com"),
                        new Claim(JwtClaimTypes.Role, "admin")
                    }
                }
            };
        }
    }
}
