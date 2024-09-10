using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace RichillCapital.Infrastructure.Identity.Server;

internal static class InMemoryClients
{
    private static readonly ICollection<Secret> DefaultSecrets =
    [
        new Secret("secret".Sha256()),
    ];

    internal static readonly IEnumerable<Client> Default =
    [
        new Client
        {
            ClientId = "RichillCapital.Identity.Web",
            ClientName = "RichillCapital Identity Web",
            Description = "RichillCapital Identity Web Client",
            ClientSecrets = DefaultSecrets,

            AllowedGrantTypes = GrantTypes.Code,

            RequirePkce = true,
            RequireClientSecret = true,
            RequireConsent = false,

            RedirectUris =
            {
                "https://localhost:9999/signin-oidc",
                "http://localhost:9999/signin-oidc",
                "https://identity.richillcapital.com/signin-oidc",
                "http://identity.richillcapital.com/signin-oidc",
            },
            PostLogoutRedirectUris =
            {
                "https://localhost:9999/signout-callback-oidc",
                "http://localhost:9999/signout-callback-oidc",
                "https://identity.richillcapital.com/signout-callback-oidc",
                "http://identity.richillcapital.com/signout-callback-oidc",
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
            ClientId = "RichillCapital.TraderStudio.Web",
            ClientName = "RichillCapital Trader Studio Web",
            Description = "RichillCapital Trader Studio Web Client",
            ClientSecrets = DefaultSecrets,

            AllowedGrantTypes = GrantTypes.Code,

            RequirePkce = true,
            RequireClientSecret = true,
            RequireConsent = false,

            RedirectUris =
            {
                "https://localhost:9998/signin-oidc",
                "http://localhost:9998/signin-oidc",
                "https://trader-studio.richillcapital.com/signin-oidc",
                "http://trader-studio.richillcapital.com/signin-oidc",
            },
            PostLogoutRedirectUris =
            {
                "https://localhost:9998/signout-callback-oidc",
                "http://localhost:9998/signout-callback-oidc",
                "https://trader-studio.richillcapital.com/signout-callback-oidc",
                "http://trader-studio.richillcapital.com/signout-callback-oidc",
            },
            AllowOfflineAccess = true,
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                IdentityServerConstants.StandardScopes.OfflineAccess,
                "RichillCapital.Api",
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
            RequireConsent = false,

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
            RequireConsent = false,

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
            RequireConsent = false,

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
            RequireConsent = false,

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
            RequireConsent = false,

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
