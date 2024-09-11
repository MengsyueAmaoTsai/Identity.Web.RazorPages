using Duende.IdentityServer.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace RichillCapital.Infrastructure.Identity.Server;

public static class IdentityServerExtensions
{
    public static IServiceCollection AddIdentityServerServices(this IServiceCollection services)
    {
        using var scope = services
            .BuildServiceProvider()
            .CreateScope();

        var identityOptions = scope.ServiceProvider
            .GetRequiredService<IOptions<IdentityOptions>>()
            .Value;

        services
            .AddIdentityServer(options =>
            {
                options.IssuerUri = identityOptions.Server.IssuerUri;

                options.UserInteraction.LoginUrl = "/sign-in";
                options.UserInteraction.LoginReturnUrlParameter = "returnUrl";

                options.UserInteraction.ErrorUrl = "/errors";
                options.UserInteraction.ErrorIdParameter = "errorId";
            })
            .AddDeveloperSigningCredential()
            .AddInMemoryClients(InMemoryClients.Default)
            .AddInMemoryApiResources(InMemoryApiResources.Default)
            .AddInMemoryApiScopes(InMemoryApiScopes.Default)
            .AddInMemoryIdentityResources(InMemoryIdentityResources.Default);

        return services;
    }

    public static bool IsNativeClient(this AuthorizationRequest context) =>
        !context.RedirectUri.StartsWith("https", StringComparison.Ordinal) &&
        !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);
}