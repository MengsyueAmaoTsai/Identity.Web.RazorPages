using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;

using FluentValidation;

using Microsoft.Extensions.Options;

using RichillCapital.Extensions.Options;

namespace RichillCapital.Identity.Web.IdentityServer;

internal static class IdentityServerExtensions
{
    internal static IServiceCollection ConfigureIdentityServer(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(
            typeof(IdentityServerExtensions).Assembly,
            includeInternalTypes: true);

        services.AddOptionsWithFluentValidation<IdentityServerOptions>(IdentityServerOptions.SectionKey);

        using var scope = services
            .BuildServiceProvider()
            .CreateScope();

        var identityServerOptions = scope.ServiceProvider
            .GetRequiredService<IOptions<IdentityServerOptions>>()
            .Value;

        services
            .AddIdentityServer(options =>
            {
                options.IssuerUri = identityServerOptions.IssuerUri;

                options.UserInteraction.LoginUrl = "/Users/SignIn";
                options.UserInteraction.LoginReturnUrlParameter = "ReturnUrl";
                options.UserInteraction.ErrorUrl = "/Error";
                options.UserInteraction.ErrorIdParameter = "ErrorId";
                options.UserInteraction.ConsentUrl = "/Users/Consent";
                options.UserInteraction.ConsentReturnUrlParameter = "ReturnUrl";

                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddInMemoryIdentityResources(InMemoryIdentityResources.Default)
            .AddInMemoryApiResources(InMemoryApiResources.Default)
            .AddInMemoryApiScopes(InMemoryApiScopes.Default)
            .AddInMemoryClients(InMemoryClients.Default);

        return services;
    }
}

internal static class InMemoryClients
{
    private static readonly ICollection<Secret> DefaultClientSecrets = [new("secret".Sha256())];

    internal static IEnumerable<Client> Default =
    [
        new()
        {
            ClientId = "Richillcapital.TraderStudio.Web.Blazor",
            ClientName = "Trader Studio Web",
            ClientSecrets = DefaultClientSecrets,
            AllowedGrantTypes = GrantTypes.Code.Combines(GrantTypes.ResourceOwnerPassword),
            RequirePkce = true,
            RedirectUris =
            {
                "https://localhost:9998/signin-oidc",
                "http://trader-studio.richillcapital.com/signin-oidc",
            },
            PostLogoutRedirectUris =
            {
                "https://localhost:9998/signout-callback-oidc",
                "http://trader-studio.richillcapital.com/signout-callback-oidc",
            },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "RichillCapital.Api.AspNetCore",
            },
            AllowRememberConsent = true,
            AllowOfflineAccess = true,
            RequireConsent = true,
        },
    ];
}

internal static class InMemoryIdentityResources
{
    internal static IEnumerable<IdentityResource> Default =
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email(),
        new IdentityResources.Phone(),
        new IdentityResources.Address(),
    ];
}

internal static class InMemoryApiScopes
{
    internal static IEnumerable<ApiScope> Default =
    [
        new ApiScope(
            name: "RichillCapital.Api.AspNetCore",
            displayName: "RichillCapital API",
            userClaims: null),
    ];
}

internal static class InMemoryApiResources
{
    internal static IEnumerable<ApiResource> Default =
    [
    ];
}

internal static class TestUsers
{
    internal static List<TestUser> Default =
    [
    ];
}
