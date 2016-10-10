﻿using System.Collections.Generic;
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
                }
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
                    AllowedScopes = { "GettoApi" }
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
                    Password = "password"
                },
                new InMemoryUser
                {
                    Subject = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
        }
    }
}