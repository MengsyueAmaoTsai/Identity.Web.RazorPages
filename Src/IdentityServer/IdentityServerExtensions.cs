using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;

namespace RichillCapital.Identity.Web.IdentityServer;

internal static class IdentityServerExtensions
{
    internal static IServiceCollection ConfigureIdentityServer(this IServiceCollection services)
    {
        services
            .AddIdentityServer()
            .AddInMemoryClients(InMemoryClients.Default)
            .AddInMemoryIdentityResources(InMemoryIdentityResources.Default)
            .AddInMemoryApiResources(InMemoryApiResources.Default)
            .AddInMemoryApiScopes(InMemoryApiScopes.Default)
            .AddTestUsers(TestUsers.Default);

        return services;
    }
}

internal static class InMemoryClients
{
    internal static IEnumerable<Client> Default =
    [
    ];
}


internal static class InMemoryIdentityResources
{
    internal static IEnumerable<IdentityResource> Default =
    [
    ];
}

internal static class InMemoryApiResources
{
    internal static IEnumerable<ApiResource> Default =
    [
    ];
}

internal static class InMemoryApiScopes
{
    internal static IEnumerable<ApiScope> Default =
    [
    ];
}

internal static class TestUsers
{
    internal static List<TestUser> Default =
    [
    ];
}