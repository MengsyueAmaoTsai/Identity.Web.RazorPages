using Duende.IdentityServer.Models;

namespace RichillCapital.Identity.Web.IdentityServer;

internal static class InMemoryIdentityResources
{
    internal static readonly IEnumerable<IdentityResource> Default = 
    [
        new IdentityResources.OpenId(),
    ];
}
