using Duende.IdentityServer.Models;

namespace RichillCapital.Infrastructure.Identity.Server;

public static class InMemoryApiResources
{
    public static IEnumerable<ApiResource> Default =
    [
        new ApiResource("RichillCapital.Api.AspNetCore", "RichillCapital API")
        {
            Description = "RichillCapital API Access",
        },
    ];
}