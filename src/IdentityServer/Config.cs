﻿using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    public class Config
    {

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("MyThings", "My Things Service")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                // OpenID Connect implicit flow client (Angular)
                new Client
                {
                    ClientId = "AngularClient",
                    ClientName = "Angular Client: Token Request, Token Validation, Token Inspection",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    ClientSecrets =
                    {
                        new Secret("AngularClient.Secret".Sha256())
                    },

                    RedirectUris = { "http://localhost:5000/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:6004/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "MyThings"
                    }
                },
                // OpenID Connect implicit flow client (WebForms)
                new Client
                {
                    ClientId = "WebFormsClient",
                    ClientName = "WebForms Client: Token Request, Token Validation, Token Inspection",
                    //AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    ClientSecrets =
                    {
                        new Secret("WebFormsClient.Secret".Sha256())
                    },
                    RedirectUris = { "http://localhost:5000/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:6000/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "MyThings"
                    }
                },
                // OpenID Connect implicit flow client (MVC)
                new Client
                {
                    ClientId = "MVCApiConsumer",
                    ClientName = "MVCApiConsumer Client: Token Request, Token Validation, Token Inspection",
                    //AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("MVCApiConsumer.Secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    RedirectUris = { "http://localhost:6004/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:6004/signout-callback-oidc" },
                    //configure which scopes will allowed be be sent to IdentityServer during authentication
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "MyThings"
                    },
                    //the offline_access scope allows requesting refresh tokens for long lived API access.
                    AllowOfflineAccess = true
                }
                //// OpenID Connect implicit flow client (WebApiClient) - a WebClient 
                //new Client
                //{
                //    ClientId = "WebApiClient",
                //    ClientName = "WebApi Client: Token Request, Token Validation, Token Inspection",
                //    AllowedGrantTypes = GrantTypes.Implicit,
                //    ClientSecrets =
                //    {
                //        new Secret("MVCApiConsumer.Secret".Sha256())
                //    },
                //    RedirectUris = { "http://localhost:5000/signin-oidc" },
                //    PostLogoutRedirectUris = { "http://localhost:6002/signout-callback-oidc" },

                //    AllowedScopes =
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile
                //    }
                //}
                //// OpenID Connect implicit flow Resource Service (MyThings API)
                //new Client
                //{
                //    ClientId = "MyThings",
                //    ClientName = "MyThings Service: Token Inspection",
                //    AllowedGrantTypes = GrantTypes.Implicit,
                //    ClientSecrets =
                //    {
                //        new Secret("MVCApiConsumer.Secret".Sha256())
                //    },
                //    RedirectUris = { "http://localhost:5000/signin-oidc" },
                //    PostLogoutRedirectUris = { "http://localhost:5001/signout-callback-oidc" },

                //    AllowedScopes =
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile
                //    }
                //}
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Alice"),
                        new Claim("website", "https://alice.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com")
                    }
                }
            };
        }

        //scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()

                //profile
                //OPTIONAL. This scope value requests access to the End-User's default profile Claims, which are: name, family_name, given_name, middle_name, nickname, preferred_username, profile, picture, website, gender, birthdate, zoneinfo, locale, and updated_at.
                //email
                //OPTIONAL. This scope value requests access to the email and email_verified Claims.
                //address
                //OPTIONAL. This scope value requests access to the address Claim.
                //phone
                //OPTIONAL. This scope value requests access to the phone_number and phone_number_verified Claims.
 
            };
        }
    }
}

