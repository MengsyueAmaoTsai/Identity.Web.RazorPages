using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;

namespace RichillCapital.Identity.Web.IdentityServer;

internal static class IdentityServerExtensions
{
    internal static IServiceCollection ConfigureIdentityServer(this IServiceCollection services)
    {
        services
            .AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/Users/SignIn";
                options.UserInteraction.LoginReturnUrlParameter = "ReturnUrl";
            })
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
    private static readonly ICollection<Secret> DefaultClientSecrets = [new("secret".Sha256())];
    internal static IEnumerable<Client> Default =
    [
        new Client
        {
        },
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