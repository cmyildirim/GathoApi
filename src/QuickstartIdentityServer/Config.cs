using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Services.InMemory;

namespace QuickstartIdentityServer
{
    public class Config
    {
        public static IEnumerable<Scope> GetScopes()
        {
            return new List<Scope>
            {
                new Scope
                {
                    Name = "GettoApi",
                    Description = "Getto API"
                },
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.OfflineAccess
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "cihangir",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("bigSecret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = new List<string>
                    {
                        "GettoApi"
                    }
                },
                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {"GettoApi"}
                },
                // OpenID Connect implicit flow client (MVC)
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    // where to redirect to after login
                    RedirectUris = {"http://localhost:5002/signin-oidc"},

                    // where to redirect to after logout
                    PostLogoutRedirectUris = {"http://localhost:5002"},
                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name
                    },
                    RequireConsent = false
                },

                //HybridFlow
                new Client
                {
                    ClientId = "mvc2",
                    ClientName = "MVC Client",
                    //we want to allow the client to use the hybrid flow, in addition we also want the client to allow doing server to server API calls which are not in the context of a user.
                    //This is expressed using the AllowedGrantTypes property.
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret2".Sha256())
                    },
                    RedirectUris = {"http://localhost:5002/signin-oidc"},
                    PostLogoutRedirectUris = {"http://localhost:5002"},
                    AllowedScopes =
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.OfflineAccess.Name,// we also give the client access to the offline_access scope - this allows requesting refresh tokens for long lived API access:
                        "GettoApi"
                    }
                }
            };
        }

        //The spec recommends using the resource owner password grant only for “trusted” (or legacy) applications.
        //Generally speaking you are typically far better off using one of the interactive OpenID Connect flows when you want to authenticate a user and request access tokens.
        public static List<InMemoryUser> GetUsers()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "1",
                    Username = "alice",
                    Password = "password",

                    Claims = new Claim[]
                    {
                        new Claim("name", "Alice"),
                        new Claim("website", "https://alice.com")
                    }
                },
                new InMemoryUser
                {
                    Subject = "2",
                    Username = "bob",
                    Password = "password",
                    Claims = new Claim[]
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com")
                    }
                }
            };
        }
    }
}