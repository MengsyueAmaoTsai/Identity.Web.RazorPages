using Duende.IdentityServer.Models;
using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.Infrastructure.Identity.Server;

public static class IdentityServerExtensions
{
    public static IServiceCollection AddIdentityServerServices(this IServiceCollection services)
    {
        services
            .AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/sign-in";
                options.UserInteraction.LoginReturnUrlParameter = "returnUrl";

                options.UserInteraction.ErrorUrl = "/error";
                options.UserInteraction.ErrorIdParameter = "errorId";
            })
            .AddInMemoryClients(InMemoryClients.Default)
            .AddInMemoryApiResources(InMemoryApiResources.Default)
            .AddInMemoryIdentityResources(InMemoryIdentityResources.Default);

        return services;
    }

    public static bool IsNativeClient(this AuthorizationRequest context) =>
        !context.RedirectUri.StartsWith("https", StringComparison.Ordinal) && 
        !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);
}