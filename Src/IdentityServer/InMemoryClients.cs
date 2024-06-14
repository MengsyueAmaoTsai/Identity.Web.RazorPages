using Duende.IdentityServer.Models;

namespace RichillCapital.Identity.Web.IdentityServer;

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
            ClientId = "RichillCapital.TraderStudio.Web.Blazor",
            ClientName = "RichillCapital Trader Studio Web",
            ClientSecrets = DefaultSecrets,

            AllowedGrantTypes = GrantTypes.Code,

            RequirePkce = true,
            RequireClientSecret = true,
            RequireConsent = false,

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
                "offline_access",
            },
        },
        new Client
        {
            ClientId = "RichillCapital.TraderStudio.Desktop.Wpf",
            ClientName = "RichillCapital Trader Studio Desktop",
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
                "profile",
                "offline_access",
            },
            AllowAccessTokensViaBrowser = true,
        },
    ];
}
