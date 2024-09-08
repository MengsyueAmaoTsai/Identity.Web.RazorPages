using Duende.IdentityServer.Models;

namespace RichillCapital.Infrastructure.Identity.Server;

internal static class InMemoryIdentityResources
{
    internal static readonly IEnumerable<IdentityResource> Default =
    [
        new IdentityResources.OpenId()
        {
            DisplayName = "Open Id",
            Description = "Your user identifier",
            Enabled = true,
            ShowInDiscoveryDocument = true,
            Required = true,
            Emphasize = false,
        },
        new IdentityResources.Profile()
        {
            DisplayName = "Profile",
            Description = "Your user profile information (first name, last name, etc.)",
            Enabled = true,
            ShowInDiscoveryDocument = true,
            Required = false,
            Emphasize = true,
        },
        new IdentityResources.Email()
        {
            DisplayName = "Email",
            Description = "Your email address",
            Enabled = true,
            ShowInDiscoveryDocument = true,
            Required = false,
            Emphasize = true,
        },
    ];
}
