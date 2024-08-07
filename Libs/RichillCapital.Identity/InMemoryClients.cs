﻿using Duende.IdentityServer.Models;

namespace RichillCapital.Identity.Web.IdentityServer;

public static class InMemoryClients
{
    private static readonly ICollection<Secret> DefaultSecrets =
    [
        new Secret("secret".Sha256()),
    ];

    public static readonly IEnumerable<Client> Default =
    [
        new Client
        {
            ClientId = "RichillCapital.Identity.Web.RazorPages",
            ClientName = "RichillCapital Identity Web",
            Description = "RichillCapital Identity Web Client",
            ClientSecrets = DefaultSecrets,

            AllowedGrantTypes = GrantTypes.Code,

            RequirePkce = true,
            RequireClientSecret = true,
            RequireConsent = true,

            RedirectUris =
            {
                "https://localhost:9999/signin-oidc",
            },
            PostLogoutRedirectUris =
            {
                "https://localhost:9999/signout-callback-oidc",
            },
            AllowOfflineAccess = true,
            AllowedScopes =
            {
                "openid",
                "profile",
            },
        },
        new Client
        {
            ClientId = "RichillCapital.TraderStudio.Web.Blazor",
            ClientName = "RichillCapital Trader Studio Web",
            Description = "RichillCapital Trader Studio Web Client",
            ClientSecrets = DefaultSecrets,

            AllowedGrantTypes = GrantTypes.Code,

            RequirePkce = true,
            RequireClientSecret = true,
            RequireConsent = true,

            RedirectUris =
            {
                "https://localhost:9998/signin-oidc",
            },
            PostLogoutRedirectUris =
            {
                "https://localhost:9998/signout-callback-oidc",
            },
            AllowOfflineAccess = true,
            AllowedScopes =
            {
                "openid",
                "profile",
            },
        },
        new Client
        {
            ClientId = "RichillCapital.Research.Web.Next",
            ClientName = "RichillCapital Research Web",
            Description = "RichillCapital Research Web Client",
            ClientSecrets = DefaultSecrets,

            AllowedGrantTypes = GrantTypes.Code,

            RequirePkce = true,
            RequireClientSecret = true,
            RequireConsent = true,

            RedirectUris =
            {
                "https://localhost:9997/signin-oidc",
            },
            PostLogoutRedirectUris =
            {
                "https://localhost:9997/signout-callback-oidc",
            },
            AllowOfflineAccess = true,
            AllowedScopes =
            {
                "openid",
                "profile",
                "email",
            },
        },

        new Client
        {
            ClientId = "RichillCapital.Exchange.Web.Angular",
            ClientName = "RichillCapital Exchange Web",
            Description = "RichillCapital Exchange Web Client",
            ClientSecrets = DefaultSecrets,

            AllowedGrantTypes = GrantTypes.Code,

            RequirePkce = true,
            RequireClientSecret = true,
            RequireConsent = true,

            RedirectUris =
            {
                "https://localhost:9996/signin-oidc",
            },
            PostLogoutRedirectUris =
            {
                "https://localhost:9996/signout-callback-oidc",
            },
            AllowOfflineAccess = true,
            AllowedScopes =
            {
                "openid",
                "profile",
                "email",
            },
        },

        new Client
        {
            ClientId = "RichillCapital.Admin.Web.Nuxt",
            ClientName = "RichillCapital Admin Web",
            ClientSecrets = DefaultSecrets,

            AllowedGrantTypes = GrantTypes.Code,

            RequirePkce = true,
            RequireClientSecret = true,
            RequireConsent = true,

            RedirectUris =
            {
                "https://localhost:9995/sign-in-callback",
                "http://localhost:9995/sign-in-callback",
                "https://admin.richillcapital.com/sign-in-callback",
            },
            PostLogoutRedirectUris =
            {
                "https://localhost:9995/signout-callback-oidc",
                "http://localhost:9995/signout-callback-oidc",
            },
            AllowOfflineAccess = true,
            AllowedScopes =
            {
                "openid",
                "profile",
                "email",
            },
        },

        new Client
        {
            ClientId = "RichillCapital.Community.Web.Astro",
            ClientName = "RichillCapital Community Web",
            Description = "RichillCapital Community Web Client",
            ClientSecrets = DefaultSecrets,
            AllowedGrantTypes = GrantTypes.Code,

            RequirePkce = true,
            RequireClientSecret = true,
            RequireConsent = true,

            RedirectUris =
            {
                "https://localhost:9994/signin-oidc",
            },
            PostLogoutRedirectUris =
            {
                "https://localhost:9994/signout-callback-oidc",
            },
            AllowOfflineAccess = true,
            AllowedScopes =
            {
                "openid",
                "profile",
                "email",
            },
        },
        new Client
        {
            ClientId = "RichillCapital.TraderStudio.Desktop.Wpf",
            ClientName = "RichillCapital Trader Studio Desktop",
            Description = "RichillCapital Trader Studio Desktop Client",
            ClientSecrets = DefaultSecrets,

            AllowedGrantTypes = GrantTypes.Code,

            RequirePkce = true,
            RequireClientSecret = false,
            RequireConsent = true,

            RedirectUris =
            {
                "richillcapital.trader-studio.desktop:/signin",
            },
            PostLogoutRedirectUris =
            {
                "richillcapital.trader-studio.desktop:/signout",
            },
            AllowOfflineAccess = true,
            AllowedScopes =
            {
                "openid",
            },
            AllowAccessTokensViaBrowser = true,
        },
    ];
}
