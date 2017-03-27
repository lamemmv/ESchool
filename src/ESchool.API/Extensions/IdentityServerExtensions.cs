//using System.Collections.Generic;
//using Microsoft.Extensions.DependencyInjection;

//namespace ESchool.API.Extensions
//{
//    public static class IdentityServerExtensions
//    {
//        public static IServiceCollection AddCustomIdentityServer(this IServiceCollection services)
//        {
//            // Configure identity server with in-memory stores, keys, clients and scopes.
//            services.AddIdentityServer()
//                .AddTemporarySigningCredential()
//                //.AddInMemoryPersistedGrants()
//                //.AddInMemoryIdentityResources(GetIdentityResources())
//                .AddInMemoryApiResources(GetApiResources())
//                .AddInMemoryClients(GetClients());
//            //.AddAspNetIdentity<ApplicationUser>()
//            //.AddInMemoryUsers(new List<InMemoryUser>());

//            return services;
//        }

//        private static IEnumerable<IdentityResource> GetIdentityResources()
//        {
//            return new List<IdentityResource>
//            {
//                new IdentityResources.OpenId(),
//                new IdentityResources.Profile(),
//                new IdentityResources.Email(),
//                new IdentityResource
//                {
//                    Name = "role",
//                    UserClaims = new List<string> { "role" }
//                }
//            };
//        }

//        private static IEnumerable<ApiResource> GetApiResources()
//        {
//            /*return new List<ApiResource> {
//                new ApiResource
//                {
//                    Name = "customAPI",
//                    DisplayName = "Custom API",
//                    Description = "Custom API Access",
//                    UserClaims = new List<string> { "role" },
//                    ApiSecrets = new List<Secret> { new Secret("scopeSecret".Sha256()) },
//                    Scopes = new List<Scope>
//                    {
//                        new Scope("customAPI.read"),
//                        new Scope("customAPI.write")
//                    }
//                }
//            };*/

//            return new List<ApiResource>
//            {
//                new ApiResource("api1", "My API")
//            };
//        }

//        private static IEnumerable<Client> GetClients()
//        {
//            //return new List<Client>
//            //{
//            //    new Client
//            //    {
//            //        ClientId = "oauthClient",
//            //        ClientName = "Example Client Credentials Client Application",
//            //        AllowedGrantTypes = GrantTypes.ClientCredentials,
//            //        ClientSecrets = new List<Secret>
//            //        {
//            //            new Secret("superSecretPassword".Sha256())
//            //        },
//            //        AllowedScopes = new List<string> {"customAPI.read"}
//            //    }
//            //};

//            return new List<Client>
//            {
//                new Client
//                {
//                    ClientId = "client",
//                    AllowedGrantTypes = GrantTypes.ClientCredentials,
//                    ClientSecrets =
//                    {
//                        new Secret("secret".Sha256())
//                    },
//                    AllowedScopes = { "api1" }
//                }
//            };
//        }
//    }
//}
