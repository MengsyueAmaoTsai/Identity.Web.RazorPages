using Duende.IdentityServer.Models;

namespace RichillCapital.Infrastructure.Identity.Server;

internal static class InMemoryApiScopes
{
    public static IEnumerable<ApiScope> Default =
    [
        new("RichillCapital.Api.AspNetCore", "RichillCapital Api"),
    ];
}