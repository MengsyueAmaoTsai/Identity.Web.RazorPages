using Duende.IdentityServer.Models;

namespace RichillCapital.Identity;

public static class InMemoryIdentityResources
{
    public static readonly IEnumerable<IdentityResource> Default =
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
    ];
}
